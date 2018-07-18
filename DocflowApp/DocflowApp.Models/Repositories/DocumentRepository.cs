﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocflowApp.Models.Filters;
using NHibernate;
using NHibernate.Criterion;

namespace DocflowApp.Models.Repositories
{
    [Repository]
    public class DocumentRepository: Repository<Document, DocumentFilter>
    {
        public DocumentRepository(ISession session) :
            base(session)
        {
        }

        public IList<Document> Find(DocumentFilter filter, FetchOptions options = null)
        {
            var crit = session.CreateCriteria<Document>();
            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.Name))
                {
                    crit.Add(Restrictions.Eq("Name", filter.Name));
                }
                if (filter.Author != null)
                {
                    crit.Add(Restrictions.Eq("Author", filter.Author));
                }
                if (filter.Date != null)
                {
                    if (filter.Date.From.HasValue)
                    {
                        crit.Add(Restrictions.Ge("WorkStartDate", filter.Date.From.Value));
                    }
                    if (filter.Date.To.HasValue)
                    {
                        crit.Add(Restrictions.Le("WorkStartDate", filter.Date.To.Value));
                    }
                }
            }
            SetupFetchOptions(crit, options);
            return crit.List<Document>();
        }
    }
}