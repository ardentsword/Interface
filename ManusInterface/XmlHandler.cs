/**
 * Copyright (C) 2015 Manus Machina
 *
 * This file is part of the Manus SDK.
 * 
 * Manus SDK is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * Manus SDK is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with Manus SDK. If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace ManusInterface
{
    class userProfileHandler{

       private XmlDocument userData;

        public userProfileHandler(string filePath)
            {
                string location = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                FileStream fs = new FileStream(location + "\\manususerdata.xml", FileMode.OpenOrCreate);
                userData = new XmlDocument();
                userData.Load(fs);
            }

        public void saveGameProfileSettings()
        {
            XmlElement newElem = userData.CreateElement("price");
            newElem.InnerText = "10.95";
            userData.DocumentElement.AppendChild(newElem);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            // Save the document to a file and auto-indent the output.
            XmlWriter writer = XmlWriter.Create("data.xml", settings);
            userData.Save(writer);
        }

        public void loadGameProfileSettings(){
            XmlNodeList xnList = userData.SelectNodes("/Names/Name[@type='M']");
            foreach (XmlNode xn in xnList)
            {
              Console.WriteLine(xn.InnerText);
            }
        }        
    }
}
