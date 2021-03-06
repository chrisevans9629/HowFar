﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HowFar.Core.Models
{
    public interface IMeasureConverters
    {
        ObjectMeasurement Centimeter { get; }
#if BRIDGE

        List<ObjectMeasurement> ObjectMeasurements { get; set; }
        List<ObjectPack> ObjectPacks { get; set; }
#else
        ObservableCollection<ObjectMeasurement> ObjectMeasurements { get; set; }
        ObservableCollection<ObjectPack> ObjectPacks { get; set; }
#endif

        double Convert(ObjectMeasurement fromMeasurement, ObjectMeasurement to, double valueFrom = 1);
        double Convert(string nameFrom, string nameTo, double valueFrom = 1);

        void NewPack(ObjectPack pack);
        
        //  double ConvertEff(string nameFrom, string nameTo, double valueFrom = 1);
        ObjectMeasurement Find(string name);
        ObjectMeasurement NewObject(string pluralName, string singleName, double value, ObjectMeasurement measurement, string pack = "Custom");
        ObjectMeasurement NewObject(string pluralName, string  singleName, double value, string measurement, string pack = "Custom");
        void DeletePack(ObjectPack pack);
        void DeleteObject(ObjectMeasurement selectedObject);
        void UpdateObject(ObjectMeasurement selectedObject);
    }
}