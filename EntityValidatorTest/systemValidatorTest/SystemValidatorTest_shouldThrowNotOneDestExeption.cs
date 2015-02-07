using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entities;
using Entities.impl;
using EntityValidator;
using EntityValidator.exeptions;
using EntityValidator.validator;

namespace EntityValidatorTest.systemValidatorTest
{
    [TestClass]
    public class SystemValidatorTest_shouldThrowNotOneDestExeption
    {
        Model instance = Model.Instance;
        
        [TestCleanup]
        public void clean()
        {
            instance.getEntities().Clear();
        }

        [TestMethod]
        [ExpectedException(typeof(NotOneDestException))]
        public void test_shouldThrowNotOneDestException()
        {
            instance.addEntity(new EntityStart());
            instance.addEntity(new EntityDestination());
            instance.addEntity(new EntityDestination());

            IValidator validator = new SystemValidator();

            validator.startValidation();
        }

    }
}
