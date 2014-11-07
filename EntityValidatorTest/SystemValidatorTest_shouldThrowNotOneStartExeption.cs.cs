using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entities;
using Entities.impl;
using EntityValidator;
using EntityValidator.exeptions;
using EntityValidator.validator;

namespace EntityValidatorTest
{
    [TestClass]
    public class SystemValidatorTest_shouldThrowNotOneDestException
    {
        Model instance = Model.Instance;

        [TestCleanup]
        public void clean()
        {
            instance.getEntities().Clear();
        }

        [TestMethod]
        [ExpectedException(typeof(NotOneStartExeption))]
        public void test_shouldThrowNotOneStartExeption()
        {
            instance.addEntity(new EntityStart());
            instance.addEntity(new EntityStart());
            instance.addEntity(new EntityDestenation());

            IValidator validator = new SystemValidator();

            validator.startValidation();
        }
    }
}
