using System.Collections;
using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class TransformTweenNode : TransformNode
    {
        private Transform _transform;

        private Transform _targetTransform;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private AnimationCurve moveCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public bool useMovement = true;
        public bool useRotation;
        public bool useScale;
        private Coroutine _coroutine;
        private IEnumerator MoveTweenEnumerator(StoryGraph storyGraph)
        {
            float elapsedTime = 0f;

            // Сохраняем начальные значения
            Vector3 startPosition = _transform.position;
            Quaternion startRotation = _transform.rotation;
            Vector3 startScale = _transform.localScale;

            while (elapsedTime < _duration)
            {
                elapsedTime += Time.deltaTime;

                // Вычисляем прогресс интерполяции
                float lerpTime = moveCurve.Evaluate(Mathf.Clamp01(elapsedTime / _duration));

                // Интерполируем на основе сохранённых значений
                if (useMovement)
                {
                    _transform.position = Vector3.LerpUnclamped(startPosition, _targetTransform.position, lerpTime);
                }
                if (useRotation)
                {
                    _transform.rotation = Quaternion.LerpUnclamped(startRotation, _targetTransform.rotation, lerpTime);
                }
                if (useScale)
                {
                    _transform.localScale = Vector3.LerpUnclamped(startScale, _targetTransform.lossyScale, lerpTime);
                }

                yield return null;
            }

            // Завершаем точно в целевом состоянии
            if (useMovement)
            {
                _transform.position = _targetTransform.position;
            }
            if (useRotation)
            {
                _transform.rotation = _targetTransform.rotation;
            }
            if (useScale)
            {
                _transform.localScale = _targetTransform.lossyScale;
            }

            OnTransitionToNextNode(storyGraph);
        }


        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _transform);
            storyGraph.GetObject(targetObjectNameID, out _targetTransform);
        }
        protected override void OnEnd(StoryGraph storyGraph)
        {
            storyGraph.StopCoroutine(_coroutine);
            _coroutine = null;
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            _coroutine = storyGraph.StartCoroutine(MoveTweenEnumerator(storyGraph));
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}