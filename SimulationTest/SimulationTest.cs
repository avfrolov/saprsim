using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entities;
using Entities.impl;
using System.Collections.Generic;
using Simulation;

namespace SimulationTest
{
    [TestClass]
    public class SimulationTest
    {

        Model instance = Model.Instance;


        [TestMethod]
        public void test_SimulateSimpleScheme()
        {
            EntityStart start = new EntityStart();
            Project prj = new Project();

            EntityDestenation finish = new EntityDestenation();
            Procedure procedure = new Procedure();

            Resource res = new Resource();
            res.efficiency = 0.8;

            procedure.addResource(res);
            procedure.setInputs(new List<Entity>() { start });
            procedure.setOutputs(new List<Entity>() { finish });
            procedure.manHour = 1;

            start.setOutputs(new List<Entity>() { procedure });
            finish.setInputs(new List<Entity>() { procedure });

            instance.addProject(prj);
            instance.addEntity(start);
            instance.addEntity(procedure);
            instance.addEntity(finish);
            

            Simulation.Simulation.simulate();

            Assert.IsTrue(prj.state == State.DONE);
        }
    }
}
