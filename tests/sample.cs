using System;  
using FoundryCore;

namespace Apprentice { 
    public class Computexx { 
        public bool getvalue(FoComponent root) { 
           
            var depthProp = new FoReference("depth"); 
            var depth = depthProp.GetValue<double>(root);
            var widthProp = new FoReference("width"); 
            var width = widthProp.GetValue<double>(root);
            var heightProp = new FoReference("height"); 
            var height = heightProp.GetValue<double>(root);
            var volumeProp = new FoReference("volume"); 
            var volume = volumeProp.GetValue<double>(root);
            var areaProp = new FoReference("area"); 
            var area = areaProp.GetValue<double>(root);

            var result = depth * ( width * height ) == volume && width * height == area;

            return result; 
        } 
    } 
}


namespace Apprentice { 
    public class Computex { 
        public bool ComputeValue (FoComponent root) { 
            var depthProp = new FoReference("depth"); 
            var depth = depthProp.GetValue<double>(root);
            var widthProp = new FoReference("width"); 
            var width = widthProp.GetValue<double>(root);
            var heightProp = new FoReference("height"); 
            var height = heightProp.GetValue<double>(root);
            var volumeProp = new FoReference("volume"); 
            var volume = volumeProp.GetValue<double>(root);
            var areaProp = new FoReference("area"); 
            var area = areaProp.GetValue<double>(root);

            var result = depth * ( width * height ) == volume && width * height == area;

            return result; 
        } 
    } 
 }