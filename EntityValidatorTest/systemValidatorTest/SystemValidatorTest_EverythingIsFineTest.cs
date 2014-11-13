using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entities;
using Entities.impl;
using EntityValidator;
using EntityValidator.exeptions;
using EntityValidator.validator;

namespace EntityValidatorTest.systemValidatorTest
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
            //creating testModel
            EntityStart start = new EntityStart();
            EntityDestenation finish = new EntityDestenation();
            Procedure procedure = new Procedure();
            Resource res = new Resource();

            procedure.addResource(res);
            procedure.setInputs(new List<Entity>(){ start });
            procedure.setOutputs(new List<Entity>() { finish });

            start.setOutputs(new List<Entity>() { procedure });
            finish.setInputs(new List<Entity>() { procedure });

            instance.addEntity(start);
            instance.addEntity(finish);
            instance.addEntity(procedure);

            IValidator validator = new SystemValidator();

            Assert.IsTrue(validator.startValidation());
        }   
        
    }
}
