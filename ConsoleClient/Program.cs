using DataAccessLayer;
using Entities;
using Entities.Base;
using Entities.Base.Utils;
using Entities.Exceptions.InnerApplicationExceptions;
using Entities.MenuUserHistory;
using MuizEnums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionERP"].ConnectionString;
            var dalc = new DALContainer(connectionString);

            var userRepository = dalc.UserRepository;
            var user = userRepository.GetItemById(55);
            //user.State = EState.Update;
            //user.Email = "123";
                        
            var userMenuItem = new UserMenuItem()
            {
                ModifyByUserID = 2,
                MenuItem = new MenuItem() { ID = 3 },
                //State = EState.Insert
            };
            //userMenuItem.ResetState();
            //userMenuItem.Fix();
            userMenuItem.State = EState.Insert;

            user.MenuHistory.Add(userMenuItem);

            var isModified = user.IsModified;

            userRepository.SaveItem(user);

            //var userSettingsRepository = dalc.UserSettingsRepository;
            //var parameters = new ParametersContainer();
            //var userSettings = userSettingsRepository.GetItems(parameters);

            var user1 = new User();

            //var mainMenuRepository = dalc.MainMenuRepository;

            //var parameters = new ParametersContainer();
            //var mainMenu = mainMenuRepository.GetItems(parameters);

            /*
            var user = new User() { };
            user.UserRoles.Add(new UserRole());
            //user.ResetState();
            user.FixValues();
            
            user.UserRoles[0].Role.Name = "123";
            user.UserRoles[0].Role.Name = null;

            var isModified = user.IsModified;
            */

            //var isModified = user.IsModified;

            //var newCompany = new Company
            //{
            //    ParentID = 4,
            //    Name = "New company 2",
            //    INN = "111111111111",
            //    OrderBy = 204,
            //    ModifyByUserID = 1
            //};

            //newCompany.FixValues();
            //newCompany.Name = "wqeqeqwe";
            //newCompany.Name = "New company 2";

            //var isChanged = newCompany.IsChanged;


            //var userRole = new UserRole() { Role = new Role() };

            //var isModified = userRole.IsModified;

            //var companyRepository = dalc.CompanyRepository;

            //var parametersContainer = new ParametersContainer();
            //parametersContainer.Add<Company>("ID", 3);

            //var companies = companyRepository.GetItems(parametersContainer);

            //var newCompany = new Company
            //{
            //    ParentID = 4,
            //    Name = "New company 2",
            //    INN = "111111111111",
            //    OrderBy = 204,
            //    ModifyUserID = 1
            //};

            //companyRepository.SaveItem(newCompany);

            //var userRepository = dalc.UserRepository;
            //var roleRepository = dalc.RoleRepository;
            //var userRoleRepository = dalc.UserRoleRepository;
            //var roleUserRepository = dalc.RoleUserRepository;

            /*var roleParameters = new ParametersContainer();
            roleParameters.Add("CompanyID", 2);*/
            //ObservableCollection<Role> roleList = roleRepository.GetItems(new ParametersContainer());
            //ObservableCollection<User> userList = userRepository.GetItems(new ParametersContainer());

            /*var newUser = new User
            {
                CompanyID = 2,
                FirstName = "Sasha",
                LastName = "Тестовая фамилия 1",
                SecondName = "Тестовое отчество 1",
                BirthDay = new DateTime(2022, 07, 05),
                Email = "test1@email.ru",
                GenderID = 0,
                Login = "login 1",
                Password = "password ",
                Phone = "+79876541232",
                Active = true,
                Color = "Color 1",
                Number = 777,
                RoleID = 1,
                ThemeID = 1,
                INN = "123456789878",
                ModifyByUserID = 1
            };

            foreach (Role role in roleList)
            {
                UserRole userRole = new UserRole();
                userRole.Role.ID = role.ID;
                userRole.IsChecked = false;
                newUser.UserRoles.Add(userRole);
            }*
            
            userRepository.SaveItem(newUser);
            */
            /*
            var newRole = new Role
            {
                CompanyID = 2,
                Name = "Программист",
                ModifyByUserID = 1
            };

            foreach (User user in userList)
            {
                RoleUser roleUser = new RoleUser();
                roleUser.User.ID = user.ID;
                roleUser.IsChecked = false;
                newRole.RoleUsers.Add(roleUser);
            }

            roleRepository.SaveItem(newRole);
            */
            //userRepository.SaveItem(newUser);
            //try
            //{
            //    var users = userRepository.GetItems(new ParametersContainer());
            //    var roles = roleRepository.GetItems(new ParametersContainer());
            //}
            //catch (SqlServerInsertRecordException ex)
            //{

            //}

            //var sampleEntityRepository = dalc.SampleEntityRepository;

            // Тестовая загрузка
            //var sampleEntities = sampleEntityRepository.GetItems();

            //// Тестовая выгрузка
            //var newSampleEntity = new SampleEntity
            //{
            //    Value = 400,
            //    Description = "Четыреста",
            //    ModifyUserID = 1,

            //    SampleEntityDetailsList = new ObservableCollection<SampleEntityDetails>
            //    {
            //        new SampleEntityDetails()
            //        {
            //            Value = 1,
            //            Description = "Детали 1",
            //            ModifyUserID = 1,
            //        },
            //        new SampleEntityDetails()
            //        {
            //            Value = 2,
            //            Description = "Детали 2",
            //            ModifyUserID = 1,
            //        },
            //        new SampleEntityDetails()
            //        {
            //            Value = 3,
            //            Description = "Детали 3",
            //            ModifyUserID = 1,
            //        }}
            //};

            //sampleEntityRepository.SaveItem(newSampleEntity);

            Console.ReadKey();
        }
    }
}
