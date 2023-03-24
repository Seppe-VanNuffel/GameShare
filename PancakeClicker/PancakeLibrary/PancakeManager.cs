﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PancakeLibrary
{
    public class PancakeManager
    {
        public decimal Money;
        private UpgradeManager UpgradeManager;
        public PancakeManager(UpgradeManager UM)
        {
            Money = 25000;
            OwnedBusinesses = new List<Business>();
            UpgradeManager = UM;
        }
        public List<Business> OwnedBusinesses { get; set; }
        public decimal Tick()
        {
            decimal moneyPerTick = 0;
            decimal moneyPerTickBusiness;
            foreach (Business business in OwnedBusinesses)
            {
                if(business == OwnedBusinesses.Find(x => x.Id == 1) && UpgradeManager.boughtUpgrades.Find(x => x.Naam == "Valuex2") != null)
                {
                    moneyPerTickBusiness = business.InitialMoneyPerSecond / 10;
                    moneyPerTickBusiness = moneyPerTickBusiness * 2;
                    moneyPerTick += moneyPerTickBusiness * business.Amount;
                }
                else if (business == OwnedBusinesses.Find(x => x.Id == 2) && UpgradeManager.boughtUpgrades.Find(x => x.Naam == "Stronger Grandma") != null)
                {
                    moneyPerTickBusiness = business.InitialMoneyPerSecond / 10;
                    moneyPerTickBusiness = moneyPerTickBusiness * 1.5M;
                    moneyPerTick += moneyPerTickBusiness * business.Amount;
                }
                else
                {
                    moneyPerTickBusiness = business.InitialMoneyPerSecond / 10;
                    moneyPerTick += moneyPerTickBusiness * business.Amount;
                }
            }
            AddMoney(moneyPerTick);
            return moneyPerTick;
        }
        public void AddMoney(decimal amount) 
        {
            Money += amount;
        }
        public void RemoveMoney(decimal amount)
        {
            Money -= amount;
        }
        public bool BuyBusinesses(int id, int amount)
        {
            int index = GetIndex(id);
            if (index < 0)
                return false;
            Business ownedBusiness = OwnedBusinesses[index];

            decimal amountOwned = CostPriceForMany(ownedBusiness, amount);
            if (Money >= amountOwned)
            {
                RemoveMoney(amountOwned);
                OwnedBusinesses[index].Amount += (uint)amount;
                return true;
            }
            else
                return false;
        }

        public decimal CostPriceForMany(Business ownedBusiness, int amount)
        {
            decimal price = 0;
            for (int i = 0; i < amount; i++)
            {
                price += CostPriceForOne(ownedBusiness);
            }
            return price;
        }

        public decimal CostPriceForOne(Business business)
        {
            decimal cost;
            int discountedPrice = 100;

            cost = (decimal)(business.InitialPrice * (decimal)Math.Pow(1.15, business.Amount));

            cost = cost * (decimal)(discountedPrice / 100);

            return cost;
        }

        public int GetIndex(int id)
        {
            return OwnedBusinesses.FindIndex(x => x.Id == id);
        }

        public void ButtonClick()
        {
            Money += UpgradeManager.ButtonClickAmount();
        }

        public void AddBusiness(Business business)
        {
            
            OwnedBusinesses.Add(business);
        }

        public bool BuyUpgrade(object OBJupgrade)
       {
            Upgrades upgrade = UpgradeManager.upgrades.FirstOrDefault(x => x.Naam == OBJupgrade.ToString());
            if(UpgradeManager.upgrades.Contains(upgrade) && Money >= upgrade.Prijs)
            {
                UpgradeManager.BuyUpgrade(upgrade);
                RemoveMoney(upgrade.Prijs);
                return true;
            }
            else
                return false;
        }
    }
}
