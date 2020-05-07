﻿using Bridge;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HowFar.Core.Models;

namespace HowFar.Bridge
{
    

    public class App 
    {
        public static void Main()
        {

            var converter = new MeasureConverters(new ObjectRepositoryCache(new AppModel()));

            

            // Write a message to the Console
            //Console.WriteLine("Welcome to Bridge.NET");

            // After building (Ctrl + Shift + B) this project, 
            // browse to the /bin/Debug or /bin/Release folder.

            // A new bridge/ folder has been created and
            // contains your projects JavaScript files. 

            // Open the bridge/index.html file in a browser by
            // Right-Click > Open With..., then choose a
            // web browser from the list

            // This application will then run in the browser.
        }

      
    }
}