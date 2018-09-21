
    class AIFloatCondition:AIConditionBase
    {
        private float value;
        public AIFloatCondition(AIConditionType aiConditionType,float targetValue) : base(aiConditionType)
        {
            value = targetValue;
        }

        protected override bool IsEquals(object obj)
        {
            //Float 不能比较等于
            return false;
        }

        protected override bool IsNotEquals(object obj)
        {
            return false;
        }

        protected override bool IsGreater(object obj)
        {
            float targetValue = (float)obj;
            return value > targetValue;
        }

        protected override bool IsLess(object obj)
        {
            float targetValue = (float)obj;
            return value < targetValue;
        }

        protected override bool IsGreaterEquals(object obj)
        {
            return false;
        }

        protected override bool IsLessEquals(object obj)
        {
            return false;
        }
    }
