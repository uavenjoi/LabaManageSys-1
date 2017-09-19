﻿using LabaManageSys.WebUI.Abstract;
using LabaManageSys.WebUI.Concrete;
using Ninject.Modules;

namespace LabaManageSys.WebUI
{
    public class RepositoryModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUsersRepository>().To<EFUsersRepository>();
            Bind<ILessonsRepository>().To<EFLessonsRepository>();
        }
    }
}
