﻿using System.Collections.ObjectModel;

namespace HowFar.Core.Models
{
    public interface IMeasureConverters
    {
        ObjectMeasurement Centimeter { get; }
        ObservableCollection<ObjectMeasurement> ObjectMeasurements { get; set; }
        ObservableCollection<ObjectPack> ObjectPacks { get; set; }
        double Convert(ObjectMeasurement from, ObjectMeasurement to, double valueFrom = 1);
        double Convert(string nameFrom, string nameTo, double valueFrom = 1);
        //  double ConvertEff(string nameFrom, string nameTo, double valueFrom = 1);
        ObjectMeasurement Find(string name);
        ObjectMeasurement NewObject(string pluralName, string singleName, double value, ObjectMeasurement measurement, string desc, string pack = "Custom");
        ObjectMeasurement NewObject(string pluralName, string  singleName, double value, string measurement, string desc, string pack = "Custom");
    }
}