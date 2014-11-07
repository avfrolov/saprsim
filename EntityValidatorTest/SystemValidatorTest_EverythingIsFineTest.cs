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
    public class SystemValidatorTest_EverythingIsFineTest
    {
        Model instance = Model.Instance;

        [TestCleanup]
        public void clean()
        {
            instance.getEntities().Clear();
        }

        [TestMethod]
        public void test_EverythingIsFineTest()
        {
            instance.addEntity(new EntityStart());
            instance.addEntity(new EntityDestenation());

            IValidator validator = new SystemValidator();

            Assert.IsTrue(validator.startValidation());
        }   
        
    }
}
