namespace DemolitionFalcons.App.Miscellaneous
{
    using DemolitionFalcons.Data;
    using DemolitionFalcons.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TakeMostPowerfulWeapon
    {
        public Weapon GetMostPowerfulWeapon(DemolitionFalconsDbContext context, int playerId)
        {
            var weapons = context.Weapons.OrderByDescending(w => w.Damage);
            var playerWeapons = context.PlayerWeapons.Where(pw => pw.PlayerId == playerId).ToList();
            var currentPlayerWeapons = new List<Weapon>();

            foreach (var weapon in weapons)
            {
                if (playerWeapons.Any(pw => pw.WeaponId == weapon.Id))
                {
                    currentPlayerWeapons.Add(weapon);
                }
            }

            var mostPowerfulWeapon = currentPlayerWeapons.OrderByDescending(cpw => cpw.Damage).FirstOrDefault();
            return mostPowerfulWeapon;
        }
    }
}
