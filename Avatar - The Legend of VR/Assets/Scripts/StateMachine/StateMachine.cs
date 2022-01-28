using System;
using System.Collections.Generic;
using System.Linq;

namespace StateMachine
{
    public class StateMachine
    {
        private IState _currentState;
        private readonly Dictionary<Type, List<Transition>> _transitions = new();
        private List<Transition> _currentTransitions = new();
        private List<Transition> _anyTransitions = new();
        private static readonly List<Transition> NoTransitions = new(0);
        
        public void Tick()
        {
            TryToTransition();
            _currentState?.Tick();
        }

        private void TryToTransition()
        {
            var anyTransition = _anyTransitions.FirstOrDefault(t => t.Predicate());
            if (anyTransition != null)
            {
                SetState(anyTransition.To);
                return;
            }

            var transition = _currentTransitions.FirstOrDefault(t => t.Predicate());
            if (transition != null) SetState(transition.To);
        }

        public void SetState(IState state)
        {
            if (state == _currentState) return;
            _currentState?.OnStateExit();
            
            state.OnStateEnter();
            _currentState = state;

            _currentTransitions = _transitions.GetValueOrDefault(state.GetType(), NoTransitions);
        }
        
        public void AddTransition(IState from, IState to, Func<bool> predicate)
        {
            var transition = new Transition(to, predicate);

            if (_transitions.TryGetValue(from.GetType(), out var transitions))
                transitions.Add(transition);
            else
                _transitions.Add(from.GetType(), new() {transition});
            
        }
        
        public void AddAnyTransition(IState to, Func<bool> predicate)
        {
            var transition = new Transition(to, predicate);
            _anyTransitions.Add(transition);
        }
        
        private class Transition
        {
            public IState To { get; }
            public Func<bool> Predicate { get; }

            public Transition(IState to, Func<bool> predicate)
            {
                To = to;
                Predicate = predicate;
            }
        }
    }
}
