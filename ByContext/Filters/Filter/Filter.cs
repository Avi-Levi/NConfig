﻿// Copyright 2011 Avi Levi

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//  http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections.Generic;
using System.Linq;
using ByContext.Filters.Evaluation;
using ByContext.Filters.Policy;

namespace ByContext.Filters.Filter
{
    public class Filter : IFilter
    {
        public Filter(IFilterPolicy policy, IFilterConditionsEvaluator filterConditionsEvaluator)
        {
            Policy = policy;
            FilterConditionsEvaluator = filterConditionsEvaluator;
        }

        private IFilterPolicy Policy { get; set; }
        private IFilterConditionsEvaluator FilterConditionsEvaluator { get; set; }
        
        public IHaveFilterConditions[] FilterItems(IDictionary<string, string> runtimeContext, IEnumerable<IHaveFilterConditions> items)
        {
            IEnumerable<ItemEvaluation> itemsEvaluationResult = this.FilterConditionsEvaluator.Evaluate(runtimeContext, items);

            return this.Policy.Filter(itemsEvaluationResult).Select(x => x.Item).ToArray();
        }
    }
}