using System;
using System.Collections.Generic;
using System.Linq;
using InventoryApp.Data;
using InventoryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Services
{
	public class InventoryService
	{
		public IEnumerable<Item> GetAll()
		{
			using var ctx = new InventoryContext();
			return ctx.Items.AsNoTracking().OrderBy(i => i.ItemNum).ToList();
		}

		public Item? FindByItemNum(string itemNum)
		{
			using var ctx = new InventoryContext();
			return ctx.Items.SingleOrDefault(i => i.ItemNum == itemNum);
		}

		public bool ItemNumExists(string itemNum)
		{
			using var ctx = new InventoryContext();
			return ctx.Items.Any(i => i.ItemNum == itemNum);
		}

		public void Add(Item item)
		{
			using var ctx = new InventoryContext();
			item.Id = 0;
			ctx.Items.Add(item);
			ctx.SaveChanges();
		}


		public void UpdateFields(int id, Action<Item> applyChanges)
		{
			using var ctx = new InventoryContext();
			var existing = ctx.Items.SingleOrDefault(i => i.Id == id);
			if (existing == null) throw new InvalidOperationException("Record not found.");
			applyChanges(existing);
			ctx.SaveChanges();
		}

		public void DeleteByItemNum(string itemNum)
		{
			using var ctx = new InventoryContext();
			var item = ctx.Items.SingleOrDefault(i => i.ItemNum == itemNum);
			if (item == null) throw new InvalidOperationException("Record not found.");
			ctx.Items.Remove(item);
			ctx.SaveChanges();
		}

		public void RemoveAll()
		{
			using var ctx = new InventoryContext();
			var all = ctx.Items.ToList();
			if (!all.Any()) return;
			ctx.Items.RemoveRange(all);
			ctx.SaveChanges();
		}

		
		public void SeedIfEmpty()
		{
			using var ctx = new InventoryContext();
			if (ctx.Items.Any()) return;

			var items = new List<Item>
			{
				new Item{ ItemNum="AH74", Description="Patience", OnHand=9, Category="GME", Storehouse=3, Price=22.99m },
				new Item{ ItemNum="BR23", Description="Skittles", OnHand=21, Category="GME", Storehouse=2, Price=29.99m },
				new Item{ ItemNum="CD33", Description="Wood Block Set (48 piece)", OnHand=36, Category="TOY", Storehouse=1, Price=89.49m },
				new Item{ ItemNum="DL51", Description="Classic Railway Set", OnHand=12, Category="TOY", Storehouse=3, Price=107.95m },
				new Item{ ItemNum="DR67", Description="Giant Star Brain Teaser", OnHand=24, Category="PZL", Storehouse=2, Price=31.95m },
				new Item{ ItemNum="DW23", Description="Mancala", OnHand=40, Category="GME", Storehouse=3, Price=50.00m },
				new Item{ ItemNum="FD11", Description="Rocking Horse", OnHand=8, Category="TOY", Storehouse=3, Price=124.95m },
				new Item{ ItemNum="FH24", Description="Puzzle Gift Set", OnHand=65, Category="PZL", Storehouse=1, Price=38.95m },
				new Item{ ItemNum="KA12", Description="Cribbage Set", OnHand=56, Category="GME", Storehouse=3, Price=75.00m },
				new Item{ ItemNum="KD34", Description="Pentominoes Brain Teaser", OnHand=60, Category="PZL", Storehouse=2, Price=14.95m },
				new Item{ ItemNum="KL78", Description="Pick Up Sticks", OnHand=110, Category="GME", Storehouse=1, Price=10.95m },
				new Item{ ItemNum="MT03", Description="Zauberkasten Brain Teaser", OnHand=45, Category="PZL", Storehouse=1, Price=45.79m },
				new Item{ ItemNum="NL89", Description="Wood Block Set (62 piece)", OnHand=32, Category="TOY", Storehouse=3, Price=119.75m },
				new Item{ ItemNum="TR40", Description="Tic Tac Toe", OnHand=75, Category="GME", Storehouse=2, Price=13.99m },
				new Item{ ItemNum="TW35", Description="Fire Engine", OnHand=30, Category="TOY", Storehouse=2, Price=118.95m }
			};

			ctx.Items.AddRange(items);
			ctx.SaveChanges();
		}
	}
}
