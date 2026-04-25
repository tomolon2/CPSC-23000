using System;
using System.Globalization;
using InventoryApp.Models;
using InventoryApp.Services;

namespace InventoryApp
{
	class Program
	{
		static InventoryService svc = new InventoryService();

		static void Main(string[] args)
		{
			Console.WriteLine("Inventory Console (SQL Server). Ensure migrations have been applied before running.");
			Console.WriteLine();

			try
			{
				
				svc.SeedIfEmpty();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error accessing database. Make sure you ran migrations and the DB is up. Details: " + ex.Message);
				return;
			}

			while (true)
			{
				Console.WriteLine();
				Console.WriteLine("Inventory Menu:");
				Console.WriteLine("S - Show all records");
				Console.WriteLine("A - Add a new record");
				Console.WriteLine("U - Update a record");
				Console.WriteLine("D - Delete a record");
				Console.WriteLine("R - Remove all records");
				Console.WriteLine("Q - Quit");
				Console.Write("Choose an option: ");
				var key = Console.ReadLine()?.Trim().ToUpperInvariant();

				switch (key)
				{
					case "S": ShowRecords(); break;
					case "A": AddRecord(); break;
					case "U": UpdateRecord(); break;
					case "D": DeleteRecord(); break;
					case "R": RemoveAll(); break;
					case "Q": return;
					default: Console.WriteLine("Unknown option."); break;
				}
			}
		}

		static void ShowRecords()
		{
			try
			{
				var items = svc.GetAll();
				if (items == null) { Console.WriteLine("No records."); return; }

				Console.WriteLine();
				Console.WriteLine("{0,-6}  {1,-40} {2,6}  {3,-4}  {4,10}", "Item#", "Description", "OnHand", "Cat", "Price");
				Console.WriteLine(new string('-', 80));
				foreach (var it in items)
				{
					var desc = it.Description.Length > 40 ? it.Description.Substring(0, 37) + "..." : it.Description;
					Console.WriteLine("{0,-6}  {1,-40} {2,6}  {3,-4}  {4,10}", it.ItemNum, desc, it.OnHand, it.Category, it.Price.ToString("F2", CultureInfo.InvariantCulture));
				}
			}
			catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }
		}

		static void AddRecord()
		{
			try
			{
				var itemNum = ReadNonEmpty("ItemNum (unique): ").ToUpperInvariant();
				if (svc.ItemNumExists(itemNum))
				{
					Console.WriteLine($"ItemNum '{itemNum}' already exists.");
					return;
				}

				var description = ReadNonEmpty("Description: ");
				var onHand = ReadInt("OnHand: ");
				var category = ReadNonEmpty("Category: ").ToUpperInvariant();
				var storehouse = ReadInt("Storehouse: ");
				var price = ReadDecimal("Price: ");

				var newItem = new Item
				{
					Id = 0,
					ItemNum = itemNum,
					Description = description,
					OnHand = onHand,
					Category = category,
					Storehouse = storehouse,
					Price = price
				};

				Console.WriteLine();
				PrintItem(newItem);

				if (Confirm("Save this record? (Y/N): "))
				{
					svc.Add(newItem);
					Console.WriteLine("Record saved.");
				}
				else Console.WriteLine("Cancelled.");
			}
			catch (Exception ex) { Console.WriteLine("Error adding record: " + ex.Message); }
		}

		static void UpdateRecord()
		{
			try
			{
				var itemNum = ReadNonEmpty("Enter ItemNum of record to update: ").ToUpperInvariant();
				var item = svc.FindByItemNum(itemNum);
				if (item == null) { Console.WriteLine("Record not found."); return; }

				Console.WriteLine("Record found:");
				PrintItem(item);

				while (true)
				{
					Console.WriteLine();
					Console.WriteLine("D - Description | O - OnHand | C - Category | S - Storehouse | P - Price | E - Exit");
					Console.Write("Choice: ");
					var ch = Console.ReadLine()?.Trim().ToUpperInvariant();
					if (string.IsNullOrEmpty(ch)) continue;
					if (ch == "E") break;

					switch (ch)
					{
						case "D":
							var newDesc = ReadNonEmpty("New Description: ");
							if (Confirm($"Change Description from '{item.Description}' to '{newDesc}'? (Y/N): "))
							{
								svc.UpdateFields(item.Id, i => i.Description = newDesc);
								Console.WriteLine("Updated.");
								item.Description = newDesc;
							}
							break;
						case "O":
							var newOn = ReadInt("New OnHand: ");
							if (Confirm($"Change OnHand from {item.OnHand} to {newOn}? (Y/N): "))
							{
								svc.UpdateFields(item.Id, i => i.OnHand = newOn);
								Console.WriteLine("Updated.");
								item.OnHand = newOn;
							}
							break;
						case "C":
							var newCat = ReadNonEmpty("New Category: ").ToUpperInvariant();
							if (Confirm($"Change Category from '{item.Category}' to '{newCat}'? (Y/N): "))
							{
								svc.UpdateFields(item.Id, i => i.Category = newCat);
								Console.WriteLine("Updated.");
								item.Category = newCat;
							}
							break;
						case "S":
							var newStore = ReadInt("New Storehouse: ");
							if (Confirm($"Change Storehouse from {item.Storehouse} to {newStore}? (Y/N): "))
							{
								svc.UpdateFields(item.Id, i => i.Storehouse = newStore);
								Console.WriteLine("Updated.");
								item.Storehouse = newStore;
							}
							break;
						case "P":
							var newPrice = ReadDecimal("New Price: ");
							if (Confirm($"Change Price from {item.Price:F2} to {newPrice:F2}? (Y/N): "))
							{
								svc.UpdateFields(item.Id, i => i.Price = newPrice);
								Console.WriteLine("Updated.");
								item.Price = newPrice;
							}
							break;
						default:
							Console.WriteLine("Unknown option.");
							break;
					}
				}
			}
			catch (Exception ex) { Console.WriteLine("Error updating record: " + ex.Message); }
		}

		static void DeleteRecord()
		{
			try
			{
				var itemNum = ReadNonEmpty("Enter ItemNum of record to delete: ").ToUpperInvariant();
				var item = svc.FindByItemNum(itemNum);
				if (item == null) { Console.WriteLine("Record not found."); return; }

				Console.WriteLine("Record to delete:");
				PrintItem(item);

				if (Confirm("Delete this record? (Y/N): "))
				{
					svc.DeleteByItemNum(itemNum);
					Console.WriteLine("Deleted.");
				}
				else Console.WriteLine("Cancelled.");
			}
			catch (Exception ex) { Console.WriteLine("Error deleting record: " + ex.Message); }
		}

		static void RemoveAll()
		{
			try
			{
				var items = svc.GetAll();
				if (items == null) { Console.WriteLine("No records."); return; }

				Console.WriteLine("All records:");
				foreach (var it in items) Console.WriteLine($"{it.ItemNum} - {it.Description} - OnHand:{it.OnHand} - {it.Category} - {it.Price:F2}");

				if (Confirm("Delete ALL records? This cannot be undone. (Y/N): "))
				{
					svc.RemoveAll();
					Console.WriteLine("All records deleted.");
				}
				else Console.WriteLine("Cancelled.");
			}
			catch (Exception ex) { Console.WriteLine("Error removing all records: " + ex.Message); }
		}

		// helpers
		static string ReadNonEmpty(string prompt)
		{
			while (true)
			{
				Console.Write(prompt);
				var s = Console.ReadLine();
				if (!string.IsNullOrWhiteSpace(s)) return s.Trim();
				Console.WriteLine("Value cannot be empty.");
			}
		}

		static int ReadInt(string prompt)
		{
			while (true)
			{
				Console.Write(prompt);
				if (int.TryParse(Console.ReadLine(), out var v)) return v;
				Console.WriteLine("Enter a valid integer.");
			}
		}

		static decimal ReadDecimal(string prompt)
		{
			while (true)
			{
				Console.Write(prompt);
				if (decimal.TryParse(Console.ReadLine(), NumberStyles.Number, CultureInfo.InvariantCulture, out var v)) return v;
				Console.WriteLine("Enter a valid decimal (e.g. 12.34).");
			}
		}

		static bool Confirm(string prompt)
		{
			while (true)
			{
				Console.Write(prompt);
				var r = Console.ReadLine()?.Trim().ToUpperInvariant();
				if (r == "Y" || r == "YES") return true;
				if (r == "N" || r == "NO") return false;
				Console.WriteLine("Enter Y or N.");
			}
		}

		static void PrintItem(Item it)
		{
			Console.WriteLine($"Id: {it.Id}");
			Console.WriteLine($"ItemNum: {it.ItemNum}");
			Console.WriteLine($"Description: {it.Description}");
			Console.WriteLine($"OnHand: {it.OnHand}");
			Console.WriteLine($"Category: {it.Category}");
			Console.WriteLine($"Storehouse: {it.Storehouse}");
			Console.WriteLine($"Price: {it.Price:F2}");
			Console.WriteLine();
		}
	}
}
