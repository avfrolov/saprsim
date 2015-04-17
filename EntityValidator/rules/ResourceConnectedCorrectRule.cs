using Entities;
using Entities.impl;
using EntityValidator.exeptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityValidator.rules
{
    public class ResourceConnectedCorrectRule : Rule
    {

        private List<Identifable> failed = new List<Identifable>();
        private List<Resource> resources = Model.Instance.getResources();

        Dictionary<WorkerResource, int> workers = new Dictionary<WorkerResource, int>();
        Dictionary<InstrumentResource, int> instruments = new Dictionary<InstrumentResource, int>();
        Dictionary<MaterialResource, int> materials = new Dictionary<MaterialResource, int>();

        public ResourceConnectedCorrectRule(List<Entity> entities, List<Resource> resources) : base(entities)
        {
            this.resources = resources;
        }

        public override bool validate()
        {
            foreach (Resource res in resources)
            {
                if (ResourceType.WORKER.Equals(res.type))
                    workers.Add(res as WorkerResource, res.count);
                else if (ResourceType.INSTRUMENT.Equals(res.type))
                    instruments.Add(res as InstrumentResource, res.count);
                else if (ResourceType.MATERIAL.Equals(res.type))
                    materials.Add(res as MaterialResource, res.count);
            }

            foreach (Entity e in entities)
            {
                if (e is Procedure)
                {
                    Procedure p = (e as Procedure);
                    foreach (Resource res in p.getResources())
                    {
                        if (ResourceType.WORKER.Equals(res.type))
                            processWorkerResources(res as WorkerResource);
                        else if (ResourceType.INSTRUMENT.Equals(res.type))
                            processInstrumentResources(new List<InstrumentResource>() { res as InstrumentResource });
                        else if (ResourceType.MATERIAL.Equals(res.type))
                            processMaterialResources(new List<MaterialResource>() { res as MaterialResource });
                    }
                }
            }

            failed.AddRange(workers.Keys);
            failed.AddRange(instruments.Keys);
            failed.AddRange(materials.Keys);

            return failed.Count == 0;
        }

        public override List<ValidationError> explain()
        {
            List<ValidationError> errors = new List<ValidationError>();
            foreach(Identifable fail in failed)
                errors.Add(new ValidationError("Ресурс '" + fail.name + "' не верно подключен", fail));            
            return errors;
        }

        private void processWorkerResources(WorkerResource wr)
        {
            processInstrumentResources(wr.instruments);
            processMaterialResources(wr.materials);

            if (workers.ContainsKey(wr))
            {
                workers[wr] -= wr.count;
                if (workers[wr] == 0)
                    workers.Remove(wr);
            }
        }

        private void processInstrumentResources(List<InstrumentResource> resources)
        {
            foreach (InstrumentResource ir in resources)
            {
                processMaterialResources(ir.materials);

                if (instruments.ContainsKey(ir))
                {
                    instruments[ir] -= ir.count;
                    if (instruments[ir] == 0)
                        instruments.Remove(ir);
                }
            }
        }

        private void processMaterialResources(List<MaterialResource> resources)
        {
            foreach (MaterialResource mr in resources)
            {
                if (materials.ContainsKey(mr))
                {
                    materials[mr] -= mr.count;
                    if (materials[mr] == 0)
                        materials.Remove(mr);
                }
            }
        }
    }
}
