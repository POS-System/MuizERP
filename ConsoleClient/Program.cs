using DataAccessLayer;
using System;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var containerDAL = new ContainerDAL();

            var sampleEntityDal = containerDAL.SampleEntityDAL;

            var sampleEntities = sampleEntityDal.GetSampleEntities();

            Console.ReadKey();
        }
    }
}
