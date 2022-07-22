﻿using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Utils;
using Entities;
using Entities.Base;
using Entities.Base.Utils.ParametersContainers;

namespace DataAccessLayer.Repositories
{
    internal class CompanyRepository : ICompanyRepository
    {
        private readonly DataRepository _dataRepository;
        private readonly IDataMapper _dataMapper;

        public CompanyRepository(
            DataRepository dataRepository,
            IDataMapper dataMapper)
        {
            _dataRepository = dataRepository;

            _dataMapper = dataMapper;
        }

        public EntityCollection<Company> GetCollection(IParametersContainer parameters)
        {
            var result = new EntityCollection<Company>();

            _dataRepository.ReadCollectionWithSchema<Company>(
                cmd => cmd.AddParameters(parameters),
                drd =>
                {
                    var item = new Company();
                    _dataMapper.Map(drd, item);

                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(Company item)
        {
            _dataRepository.DoInConnectionSession(
                conn => _dataRepository.SaveBaseItem(item, conn));
        }
    }
}
