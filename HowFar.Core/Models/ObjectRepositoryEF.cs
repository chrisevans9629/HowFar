﻿using System;
using System.Collections.Generic;
using System.Linq;
using HowFarApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HowFar.Core.Models
{
    public class ObjectRepositoryEF : IObjectRepository
    {
        private readonly DatabaseContext db;

        public ObjectRepositoryEF(DatabaseContext dbcontext)
        {
            db = dbcontext;
            ObjectRepositorySeeder.Startup(this);
        }




        public void UpdatePack(string pack, ObjectMeasurement newObject)
        {
            var packs = db.ObjectPacks.FirstOrDefault(p => p.PackName == pack);
            if (packs != null)
            {
                if (newObject.Measurement != null)
                {
                    db.Entry(newObject.Measurement).State = EntityState.Modified;
                }
                packs.ObjectMeasurements.Add(newObject);
            }
            else
            {
                throw new InvalidOperationException($"package '{pack}' must exist first");
            }

            Save();

        }

        private void Save()
        {
            if (ShouldSave)
                db.SaveChanges();
        }

        public ObjectMeasurement NewObject(string pluralName, string singleName, double value, string measurementStr, string pack = "Custom")
        {
            return ObjectRepositorySeeder.NewObjectAction(pluralName, singleName, value, measurementStr, pack, this);

        }



        public ObjectMeasurement NewObject(string pluralName, string singleName,
            double value, ObjectMeasurement measurement, string pack = "Custom")
        {
            return NewObject(pluralName, singleName, value, measurement?.PluralName, pack);
        }


        public bool ShouldSave { get; set; } = true;

        public ObjectMeasurement GetObjectMeasurement(string name)
        {
            var data = GetObjectMeasurements();
            return data.FirstOrDefault(p => p.SingleName.ToLower() == name.ToLower() || p.PluralName.ToLower() == name.ToLower());
        }



        public IEnumerable<ObjectMeasurement> GetObjectMeasurements()
        {
            var data = db.ObjectMeasurements.ToList().Union(db.ObjectMeasurements.Local).ToList();
            return ObjectRepositorySeeder.SetupTree(data);
        }

        public void AddObject(ObjectMeasurement measurement)
        {
            db.ObjectMeasurements.Add(measurement);
            Save();
        }

        public void RemoveObject(ObjectMeasurement measurement)
        {
            db.ObjectMeasurements.Remove(measurement);
            Save();
        }



        public IEnumerable<ObjectPack> GetObjectPacks()
        {
            return db.ObjectPacks;
        }

        public void AddPack(ObjectPack pack)
        {
            db.ObjectPacks.Add(pack);
            Save();
        }

        public void RemovePack(ObjectPack pack)
        {
            db.ObjectPacks.Remove(pack);
            Save();
        }

        public void UpdateObject(ObjectMeasurement selectedObject)
        {
            db.ObjectMeasurements.Update(selectedObject);
            Save();
        }

        public void Dispose()
        {
            db?.Dispose();
        }

        public ObjectPack GetObjectPack(string pack)
        {
            return db.ObjectPacks.FirstOrDefault(p => p.PackName == pack);
        }
    }
}