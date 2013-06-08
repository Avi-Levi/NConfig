﻿using System.Collections.Generic;
using ByContext.ConfigurationDataProviders;
using ByContext.Filters.Conditions.TextMatch;
using ByContext.Filters.Evaluation;
using ByContext.Filters.Policy;
using ByContext.ModelBinders;
using ByContext.ResultBuilder;
using ByContext.StringToValueTranslator;
using ByContext.StringToValueTranslator.SerializeStringToValueTranslator;

namespace ByContext
{
    public class ByContextSettings : IByContextSettings
    {
        public ByContextSettings()
        {
            this.ModelBinderFactory = new DynamicMethodModelBinderFactory();
            this.FilterConditionsEvaluator = new FilterConditionsEvaluator();
            this.ResultBuilderProvider = new ResultBuilderProvider();

            this.FilterPolicies = new Dictionary<string, IFilterPolicy>();
            this.ConfigurationDataProviders = new List<IConfigurationDataProvider>();
            this.RuntimeContext = new Dictionary<string, string>();
            this.InitTranslatorProviders();
            this.InitFilterConditionFactories();

            this.SetSingleValueDefaultFilterPolicy();
            this.SetCollectionDefaultFilterPolicy();
            this.SetDefaultRawValueTranslatorName();
            this.SetDefaultFilterConditionName();

            this.ThrowIfParameterMemberMissing = false;
        }
        public IDictionary<string, string> RuntimeContext { get; set; }
        public IList<IConfigurationDataProvider> ConfigurationDataProviders { get; private set; }
        public IDictionary<string, IStringToValueTranslatorProvider> TranslatorProviders { get; private set; }
        public IDictionary<string, IFilterConditionFactory> FilterConditionFactories { get; private set; }
        public string DefaultRawValueTranslatorName { get; set; }
        public string DefaultFilterConditionName { get; set; }

        public IModelBinderFactory ModelBinderFactory { get; set; }
        public IDictionary<string, IFilterPolicy> FilterPolicies { get; private set; }
        public IFilterConditionsEvaluator FilterConditionsEvaluator { get; set; }
        public ResultBuilderProvider ResultBuilderProvider { get; private set; }
        public bool ThrowIfParameterMemberMissing { get; set; }

        #region private methods

        private void InitFilterConditionFactories()
        {
            this.FilterConditionFactories = new Dictionary<string, IFilterConditionFactory>
                                                {
                                                    {TextMatchCondition.Name,new TextMatchConditionFactory()}
                                                };
        }
        private void InitTranslatorProviders()
        {
            this.TranslatorProviders = new Dictionary<string, IStringToValueTranslatorProvider>();
            this.AddTranslatorProvider(SerializeStringToValueTranslatorProvider.ProviderKey, new SerializeStringToValueTranslatorProvider());
        }

        private void SetCollectionDefaultFilterPolicy()
        {
            this.FilterPolicies.Add(Configure.DefaultCollectionFilterPolicyName, new SelectAllRelevantFilterPolicy());
        }
        private void SetSingleValueDefaultFilterPolicy()
        {
            this.FilterPolicies.Add(Configure.DefaultSingleValueFilterPolicyName, new BestMatchFilterPolicy());
        }

        private void SetDefaultRawValueTranslatorName()
        {
            this.DefaultRawValueTranslatorName = SerializeStringToValueTranslatorProvider.ProviderKey;
        }

        private void SetDefaultFilterConditionName()
        {
            this.DefaultFilterConditionName = TextMatchCondition.Name;
        }

        #endregion private methods
    }
}