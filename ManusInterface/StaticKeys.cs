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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManusInterface
{
    /**
     * Holds all static keys to be used when testing and validating responses from the Docking station
     **/
    static class StaticKeys
    {
        public static String DETECT_LEFT = "GLOVE_L";
        public static String DETECT_RIGHT = "GLOVE_R";
        public static String DETECT_DOCK = "DOCK";
        public static String AOK = "AOK";
        public static String ERR = "ERR";
    }
}
