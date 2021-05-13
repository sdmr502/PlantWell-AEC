using Microsoft.Extensions.Configuration;
using PlantWellBgClient.DAL;
using PlantWellBgClient.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantWellBgClient.Logic
{
    class Controller
    {
        private MatrixPrediction est;
        private static IConfiguration _iconfiguration;
        private double predictResult = 0;


        public Controller(IConfiguration iconfiguration) {
            _iconfiguration = iconfiguration;
        }
        /**
        public static void correctBalance(EnvironmentData env, string use) {
            List<MatrixPrediction> est = new List<MatrixPrediction>();
            MatrixPrediction index;
            // we are going to ignore the first digit in our option.
            //var environmentDAL = new MatrixDAL(_iconfiguration);
            //var listEnvironmentModel = environmentDAL.GetAllList();
            listEnvironmentModel.ForEach(item =>
            {
                Debug.WriteLine(item);
                //Create a prediction method here and pass through the use command
                est.Add(new MatrixPrediction()
                {
                    Id = 0,
                    Prediction = 0,
                    IntakeFan = false,
                    OuttakeFan = false,
                    Lighting = false,
                    Dehumidifer = false
                });
            });
            est.Sort((s1, s2) => s1.Prediction.CompareTo(s2.Prediction));
            int[] option = env.Option.ToString().Select(o => Convert.ToInt32(o)).ToArray();
            Debug.WriteLine(option);
            if (use == "TempPredict") {
                //Add 1 to method which we can use the cloest method
                switch (option[0])
                {
                    case 2:
                        index = est.First();
                        Debug.WriteLine("Temperature Case 2");
                        break;
                    case 3:
                        index = est.Last();
                        Debug.WriteLine("Temperature Case 3");
                        break;
                    default:
                        Debug.WriteLine("Temperature Default case");
                        break;
                }
            }
            else if (use == "HumPredict")
            {
                switch (option[1])
                {
                    case 2:
                        index = est.Last();
                        Debug.WriteLine("Humidity Case 2");
                        break;
                    case 3:
                        index = est.First();
                        Debug.WriteLine("Humidity Case 3");
                        break;
                    default:
                        Debug.WriteLine("Humidity Default case");
                        break;
                }

            }
        }

        **/
        public static bool Between(float num, float lower, float upper, bool inclusive = false)
        {
            return inclusive
                ? lower <= num && num <= upper
                : lower < num && num < upper;
        }

        public static int Balancer(float actual, float targetMin, float targetMax)
        {
            //The function allows us to simplify the results to determine the best method more dynamically
            if (Between(actual, targetMin, targetMax))
            {
                return 1;
            } 
            else if(actual > targetMin && actual > targetMax) {
                return 2;
            }
            else if(actual < targetMin && actual < targetMax) {
                return 3;
            }
            return 0;
        }


        public static bool WithinThreshold(float threshold, float target, float actual)
        {

            float upper = target + threshold;
            float lower = target - threshold;

            if (actual is not 0)
            {
             if (Between(actual,lower,upper))
                {
                    return true;
                }
            }
            return false;
        }

        public static float GetThreshold(float percentage, float of)
        {
            return ((percentage * of) / 100);
        }


        public static float ReturnThreshold(float threshold, float target, string tag) {
            if (tag == "-"){
                return (target - threshold);
            }
            return (target + threshold);
       
        }
    }
}

