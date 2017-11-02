using System;
using System.Collections.Generic;

namespace Ubora.Web.Infrastructure.PreMailers
{
    public class PreMailerFactory : IDisposable
    {
        private readonly List<PreMailerWrapper> _createdInstances = new List<PreMailerWrapper>();

        public virtual PreMailerWrapper Create(string html, Uri baseUri = null)
        {
            var instance = new PreMailerWrapper(new PreMailer.Net.PreMailer(html, baseUri));

            _createdInstances.Add(instance);

            return instance;
        }

        public void Dispose()
        {
            foreach (var instance in _createdInstances)
            {
                instance.Dispose();
            }
        }
    }
}