using System;

namespace Cepedi.ProjetoRFID.Leitura.Domain.Notifications;

public class Notification
    {
        public Notification()
        {
        }

        public Notification(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
