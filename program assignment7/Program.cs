using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;

class Vendor { public int Id; public string Name; public string Contact; public string Area; public string Phone; public string State; public string Preferred; }
class Product { public string Code; public string Description; public DateTime? LastReceived; public int OnHand; public int Reorder; public decimal Price; public decimal Discount; public int? VendorId; }
class Customer { public int Id; public string Last; public string First; public string Middle; public string Area; public string Phone; public decimal Balance; }
class Invoice { public int Id; public int CustomerId; public DateTime? Date; }
class Line { public int InvoiceId; public int LineNo; public string ProductCode; public int Quantity; public decimal UnitPrice; }

class Program
{
	static List<string> SplitFields(string line)
	{
		var res = new List<string>();
		int i = 0;
		while (i < line.Length)
		{
			while (i < line.Length && char.IsWhiteSpace(line[i])) i++;
			if (i >= line.Length) break;
			if (line[i] == '\'')
			{
				i++;
				string sb = "";
				while (i < line.Length)
				{
					if (line[i] == '\'' && i + 1 < line.Length && line[i + 1] == '\'') { sb += '\''; i += 2; continue; }
					if (line[i] == '\'') { i++; break; }
					sb += line[i++];
				}
				res.Add(sb.Trim());
				while (i < line.Length && line[i] != ',') i++;
				if (i < line.Length && line[i] == ',') i++;
			}
			else
			{
				int start = i;
				while (i < line.Length && line[i] != ',') i++;
				res.Add(line.Substring(start, i - start).Trim());
				if (i < line.Length && line[i] == ',') i++;
			}
		}
		return res;
	}

	static void RunSqlFileToCsv(SqliteConnection conn, string sqlFile, string outFile)
	{
		var content = File.ReadAllText(sqlFile);
		var parts = content.Split(';').Select(p => p.Trim()).Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
		using var sw = new StreamWriter(outFile, false);
		bool firstResult = true;
		foreach (var part in parts)
		{
			using var cmd = conn.CreateCommand();
			cmd.CommandText = part;
			try
			{
				using var reader = cmd.ExecuteReader();
				if (reader.FieldCount > 0)
				{
					if (!firstResult) sw.WriteLine();
					for (int i = 0; i < reader.FieldCount; i++)
					{
						if (i > 0) sw.Write(",");
						sw.Write(reader.GetName(i));
					}
					sw.WriteLine();
					while (reader.Read())
					{
						for (int i = 0; i < reader.FieldCount; i++)
						{
							if (i > 0) sw.Write(",");
							var val = reader.IsDBNull(i) ? "" : reader.GetValue(i).ToString();
							sw.Write(val);
						}
						sw.WriteLine();
					}
					firstResult = false;
				}
			}
			catch
			{
				
			}
		}
	}

	static int Main(string[] args)
	{
		var dataFile = "SalesComp_Data.txt";
		if (!File.Exists(dataFile))
		{
			Console.WriteLine("SalesComp_Data.txt not found.");
			return 1;
		}

		var vendors = new List<Vendor>();
		var products = new List<Product>();
		var customers = new List<Customer>();
		var invoices = new List<Invoice>();
		var lines = new List<Line>();

		string section = null;
		foreach (var raw in File.ReadAllLines(dataFile))
		{
			var l = raw.Trim();
			if (string.IsNullOrEmpty(l)) continue;
			if (l.IndexOf("VENDOR", StringComparison.OrdinalIgnoreCase) >= 0) { section = "VENDOR"; continue; }
			if (l.IndexOf("PRODUCT", StringComparison.OrdinalIgnoreCase) >= 0) { section = "PRODUCT"; continue; }
			if (l.IndexOf("CUSTOMER", StringComparison.OrdinalIgnoreCase) >= 0) { section = "CUSTOMER"; continue; }
			if (l.IndexOf("INVOICE", StringComparison.OrdinalIgnoreCase) >= 0) { section = "INVOICE"; continue; }
			if (l.IndexOf("LINE", StringComparison.OrdinalIgnoreCase) >= 0) { section = "LINE"; continue; }
			if (l.StartsWith("/*") || l.StartsWith("*/") || l.StartsWith("*")) continue;

			var f = SplitFields(l);
			if (section == "VENDOR")
			{
				if (f.Count < 7) continue;
				if (!int.TryParse(f[0], out var id)) continue;
				vendors.Add(new Vendor { Id = id, Name = f[1], Contact = f[2], Area = f[3].Trim('\''), Phone = f[4], State = f[5], Preferred = f[6] });
			}
			else if (section == "PRODUCT")
			{
				if (f.Count < 8) continue;
				DateTime? d = null;
				if (DateTime.TryParseExact(f[2].Trim('\''), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt)) d = dt;
				if (!int.TryParse(f[3], out var reorder)) continue;
				if (!int.TryParse(f[4], out var onhand)) continue;
				if (!decimal.TryParse(f[5], NumberStyles.Any, CultureInfo.InvariantCulture, out var price)) continue;
				decimal disc = 0; decimal.TryParse(f[6], NumberStyles.Any, CultureInfo.InvariantCulture, out disc);
				int? vid = null; if (int.TryParse(f[7], out var v)) vid = v;
				products.Add(new Product { Code = f[0], Description = f[1], LastReceived = d, Reorder = reorder, OnHand = onhand, Price = price, Discount = disc, VendorId = vid });
			}
			else if (section == "CUSTOMER")
			{
				if (f.Count < 7) continue;
				if (!int.TryParse(f[0], out var id)) continue;
				decimal bal = 0; decimal.TryParse(f[6], NumberStyles.Any, CultureInfo.InvariantCulture, out bal);
				customers.Add(new Customer { Id = id, Last = f[1], First = f[2], Middle = f[3] == "NULL" ? null : f[3], Area = f[4], Phone = f[5], Balance = bal });
			}
			else if (section == "INVOICE")
			{
				if (f.Count < 3) continue;
				if (!int.TryParse(f[0], out var id)) continue;
				if (!int.TryParse(f[1], out var cid)) continue;
				DateTime? d = null; if (DateTime.TryParseExact(f[2].Trim('\''), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt)) d = dt;
				invoices.Add(new Invoice { Id = id, CustomerId = cid, Date = d });
			}
			else if (section == "LINE")
			{
				if (f.Count < 5) continue;
				if (!int.TryParse(f[0], out var iid)) continue;
				if (!int.TryParse(f[1], out var ln)) continue;
				if (!int.TryParse(f[3], out var qty)) continue;
				if (!decimal.TryParse(f[4], NumberStyles.Any, CultureInfo.InvariantCulture, out var up)) continue;
				lines.Add(new Line { InvoiceId = iid, LineNo = ln, ProductCode = f[2], Quantity = qty, UnitPrice = up });
			}
		}

		var dbFile = "SalesComp.db";
		if (File.Exists(dbFile)) File.Delete(dbFile);
		using var conn = new SqliteConnection($"Data Source={dbFile}");
		conn.Open();

		using (var cmd = conn.CreateCommand())
		{
			cmd.CommandText =
@"CREATE TABLE Vendor (vendor_id INTEGER PRIMARY KEY, vendor_name TEXT, contact_last TEXT, area_code TEXT, phone TEXT, state TEXT, preferred TEXT);
CREATE TABLE Product (product_code TEXT PRIMARY KEY, description TEXT, last_received TEXT, reorder_point INTEGER, on_hand INTEGER, price REAL, discount REAL, vendor_id INTEGER);
CREATE TABLE Customer (customer_id INTEGER PRIMARY KEY, last_name TEXT, first_name TEXT, middle_initial TEXT, area_code TEXT, phone TEXT, balance REAL);
CREATE TABLE Invoice (invoice_id INTEGER PRIMARY KEY, customer_id INTEGER, invoice_date TEXT);
CREATE TABLE Line (invoice_id INTEGER, line_no INTEGER, product_code TEXT, quantity INTEGER, unit_price REAL, PRIMARY KEY(invoice_id,line_no));";
			cmd.ExecuteNonQuery();
		}

		using (var tran = conn.BeginTransaction())
		{
			var vcmd = conn.CreateCommand();
			vcmd.CommandText = "INSERT INTO Vendor(vendor_id,vendor_name,contact_last,area_code,phone,state,preferred) VALUES ($id,$name,$contact,$area,$phone,$state,$pref)";
			vcmd.Parameters.Add("$id", SqliteType.Integer);
			vcmd.Parameters.Add("$name", SqliteType.Text);
			vcmd.Parameters.Add("$contact", SqliteType.Text);
			vcmd.Parameters.Add("$area", SqliteType.Text);
			vcmd.Parameters.Add("$phone", SqliteType.Text);
			vcmd.Parameters.Add("$state", SqliteType.Text);
			vcmd.Parameters.Add("$pref", SqliteType.Text);
			foreach (var v in vendors)
			{
				vcmd.Parameters["$id"].Value = v.Id;
				vcmd.Parameters["$name"].Value = v.Name;
				vcmd.Parameters["$contact"].Value = v.Contact;
				vcmd.Parameters["$area"].Value = v.Area;
				vcmd.Parameters["$phone"].Value = v.Phone;
				vcmd.Parameters["$state"].Value = v.State;
				vcmd.Parameters["$pref"].Value = v.Preferred;
				vcmd.ExecuteNonQuery();
			}

			var pcmd = conn.CreateCommand();
			pcmd.CommandText = "INSERT INTO Product(product_code,description,last_received,reorder_point,on_hand,price,discount,vendor_id) VALUES ($code,$desc,$lr,$re,$on,$pr,$di,$vid)";
			pcmd.Parameters.Add("$code", SqliteType.Text);
			pcmd.Parameters.Add("$desc", SqliteType.Text);
			pcmd.Parameters.Add("$lr", SqliteType.Text);
			pcmd.Parameters.Add("$re", SqliteType.Integer);
			pcmd.Parameters.Add("$on", SqliteType.Integer);
			pcmd.Parameters.Add("$pr", SqliteType.Real);
			pcmd.Parameters.Add("$di", SqliteType.Real);
			pcmd.Parameters.Add("$vid", SqliteType.Integer);
			foreach (var p in products)
			{
				pcmd.Parameters["$code"].Value = p.Code;
				pcmd.Parameters["$desc"].Value = p.Description;
				pcmd.Parameters["$lr"].Value = p.LastReceived?.ToString("dd-MMM-yyyy") ?? "";
				pcmd.Parameters["$re"].Value = p.Reorder;
				pcmd.Parameters["$on"].Value = p.OnHand;
				pcmd.Parameters["$pr"].Value = p.Price;
				pcmd.Parameters["$di"].Value = p.Discount;
				pcmd.Parameters["$vid"].Value = p.VendorId.HasValue ? (object)p.VendorId.Value : DBNull.Value;
				pcmd.ExecuteNonQuery();
			}

			var ccmd = conn.CreateCommand();
			ccmd.CommandText = "INSERT INTO Customer(customer_id,last_name,first_name,middle_initial,area_code,phone,balance) VALUES ($id,$ln,$fn,$mi,$area,$phone,$bal)";
			ccmd.Parameters.Add("$id", SqliteType.Integer);
			ccmd.Parameters.Add("$ln", SqliteType.Text);
			ccmd.Parameters.Add("$fn", SqliteType.Text);
			ccmd.Parameters.Add("$mi", SqliteType.Text);
			ccmd.Parameters.Add("$area", SqliteType.Text);
			ccmd.Parameters.Add("$phone", SqliteType.Text);
			ccmd.Parameters.Add("$bal", SqliteType.Real);
			foreach (var c in customers)
			{
				ccmd.Parameters["$id"].Value = c.Id;
				ccmd.Parameters["$ln"].Value = c.Last;
				ccmd.Parameters["$fn"].Value = c.First;
				ccmd.Parameters["$mi"].Value = c.Middle ?? "";
				ccmd.Parameters["$area"].Value = c.Area;
				ccmd.Parameters["$phone"].Value = c.Phone;
				ccmd.Parameters["$bal"].Value = c.Balance;
				ccmd.ExecuteNonQuery();
			}

			var icmd = conn.CreateCommand();
			icmd.CommandText = "INSERT INTO Invoice(invoice_id,customer_id,invoice_date) VALUES ($id,$cid,$dt)";
			icmd.Parameters.Add("$id", SqliteType.Integer);
			icmd.Parameters.Add("$cid", SqliteType.Integer);
			icmd.Parameters.Add("$dt", SqliteType.Text);
			foreach (var inv in invoices)
			{
				icmd.Parameters["$id"].Value = inv.Id;
				icmd.Parameters["$cid"].Value = inv.CustomerId;
				icmd.Parameters["$dt"].Value = inv.Date?.ToString("dd-MMM-yyyy") ?? "";
				icmd.ExecuteNonQuery();
			}

			var lcmd = conn.CreateCommand();
			lcmd.CommandText = "INSERT INTO Line(invoice_id,line_no,product_code,quantity,unit_price) VALUES ($iid,$ln,$pc,$qty,$up)";
			lcmd.Parameters.Add("$iid", SqliteType.Integer);
			lcmd.Parameters.Add("$ln", SqliteType.Integer);
			lcmd.Parameters.Add("$pc", SqliteType.Text);
			lcmd.Parameters.Add("$qty", SqliteType.Integer);
			lcmd.Parameters.Add("$up", SqliteType.Real);
			foreach (var ln in lines)
			{
				lcmd.Parameters["$iid"].Value = ln.InvoiceId;
				lcmd.Parameters["$ln"].Value = ln.LineNo;
				lcmd.Parameters["$pc"].Value = ln.ProductCode;
				lcmd.Parameters["$qty"].Value = ln.Quantity;
				lcmd.Parameters["$up"].Value = ln.UnitPrice;
				lcmd.ExecuteNonQuery();
			}

			tran.Commit();
		}

		var queries = new (string SqlFile, string OutFile)[]
		{
			("query1.sql","report1.txt"),
			("query2.sql","report2.txt"),
			("query3.sql","report3.txt"),
			("query4.sql","report4.txt"),
			("query5.sql","report5.txt")
		};

		for (int i = 0; i < queries.Length; i++)
		{
			var q = queries[i];
			if (!File.Exists(q.SqlFile))
			{
				Console.WriteLine($"Missing {q.SqlFile}");
				continue;
			}
			RunSqlFileToCsv(conn, q.SqlFile, q.OutFile);
		}

		Console.WriteLine("Reports written: report1.txt ... report5.txt (SQL used).");
		return 0;
	}
}
