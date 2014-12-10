﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Client;

namespace NuGet.Client
{
    /// <summary>
    /// V2SourceRepository which exposes various resources like SearchResource, MetricsResource and so on...
    /// *TODOS: Add tracing
    /// </summary>
    public class V2SourceRepository2 : SourceRepository2
    {
        private readonly IPackageRepository _repository;
        private readonly LocalPackageRepository _lprepo;
        private readonly PackageSource _source;
        private readonly string _userAgent;

        public override PackageSource Source { get { return _source; } }

        public V2SourceRepository2(PackageSource source, IPackageRepository repository, string host)
        {
            _source = source;
            _repository = repository;

            // TODO: Get context from current UI activity (PowerShell, Dialog, etc.)
            _userAgent = UserAgentUtil.GetUserAgent("NuGet.Client.Interop", host);

            var events = _repository as IHttpClientEvents;
            if (events != null)
            {
                events.SendingRequest += (sender, args) =>
                {
                    var httpReq = args.Request as HttpWebRequest;
                    if (httpReq != null)
                    {
                        httpReq.UserAgent = _userAgent;
                    }                    
                };
            }

            _lprepo = _repository as LocalPackageRepository;
//            AddResource<SearchResource>(() => new V2SearchResource(repository,host));
        }

       
    }
}