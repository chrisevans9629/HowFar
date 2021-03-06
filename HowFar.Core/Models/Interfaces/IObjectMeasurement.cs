﻿using System.Collections.Generic;

namespace HowFar.Core.Models
{
    public interface IObjectMeasurement
    {
        string Image { get; set; }
        ObjectMeasurement Measurement { get; set; }
        string PluralName { get;  }
        string SingleName { get; }
        double Value { get; set; }

        void Add(ObjectMeasurement obj);
        //IEnumerable<ObjectMeasurement> GetChildren();
        string ToString();
    }
}