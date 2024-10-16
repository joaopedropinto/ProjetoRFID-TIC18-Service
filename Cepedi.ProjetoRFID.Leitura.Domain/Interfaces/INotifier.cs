using System.Collections.Generic;
using Cepedi.ProjetoRFID.Leitura.Domain.Notifications;


namespace Cepedi.ProjetoRFID.Leitura.Domain.Interfaces;

public interface INotifier
    {
        bool HasNotification();
        List<Notification> GetNotifications();
        void Handle(Notification notification);
        void Handle(string notification);
    }