using System;

namespace Cepedi.ProjetoRFID.Leitura.Domain.Entities;

public abstract class Entity
    {
        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }
