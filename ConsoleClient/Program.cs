using DataAccessLayer;
using Entities.SampleEntity;
using System;
using System.Collections.Generic;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var containerDAL = new ContainerDAL();

            var sampleEntityDAL = containerDAL.SampleEntityDAL;

            // Тестовая загрузка
            // var sampleEntities = sampleEntityDAL.GetSampleEntities();

            var newSampleEntityDAL = new SampleEntity
            {
                Value = 400,
                Description = "Четыреста",
                LastModifiedDate = DateTime.Now,
                LastModifiedByUserID = 1,
                SampleEntityDetailsList = new List<SampleEntityDetails>
                {
                    new SampleEntityDetails()
                    {
                        Value = 1,
                        Description = "Детали 1",
                        LastModifiedDate = DateTime.Now,
                        LastModifiedByUserID = 1
                    },
                    new SampleEntityDetails()
                    {
                        Value = 2,
                        Description = "Детали 2",
                        LastModifiedDate = DateTime.Now,
                        LastModifiedByUserID = 1
                    },
                    new SampleEntityDetails()
                    {
                        Value = 3,
                        Description = "Детали 2",
                        LastModifiedDate = DateTime.Now,
                        LastModifiedByUserID = 1
                    }}
            };

            // Тестовое сохранение
            sampleEntityDAL.SetSampleEntity(newSampleEntityDAL);


            Console.ReadKey();
        }
    }
}
