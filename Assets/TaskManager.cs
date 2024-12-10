using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TaskManager : MonoBehaviour
{
    public enum TaskType
    {
        Delivery
    }
    public enum TaskComplexity
    {
        Low, Medium, High
    }

    public class TaskObject
    {
		delegate void TaskLauncher(TaskObject task);
		class TypeInfo
        {
            public int Chance { get; }
            public int Award { get; }

            public TaskLauncher Launcher { get; }

			public TypeInfo(int chance, int award, TaskLauncher launcher)
            {
                this.Chance = chance;
                this.Award = award;
                this.Launcher = launcher;
            }
        }

        class ComplexityInfo
        {
            public int Chance { get; }
            public int Award { get; }

            public ComplexityInfo(int chance, int award)
            {
                this.Chance = chance;
                this.Award = award;
            }
        }

        Dictionary<TaskType, TypeInfo> types = new Dictionary<TaskType, TypeInfo>()
        {
            {TaskType.Delivery, new TypeInfo(40, 500, new Launchers().LaunchDelivery) }
        };

        Dictionary<TaskComplexity, ComplexityInfo> complexities = new Dictionary<TaskComplexity, ComplexityInfo>()
        {
            {TaskComplexity.Low, new ComplexityInfo(50, 500) },
            {TaskComplexity.Medium, new ComplexityInfo(80, 600) },
            {TaskComplexity.High, new ComplexityInfo(100, 800) }
        };

        public TaskType Type {  get; }
        public TaskComplexity Complexity { get; }
        public int Award { get; }
        private TaskLauncher launcher;

        public void LaunchTask()
        {
            launcher(this);
        }

        public TaskObject()
        {
            int typeChance = Random.Range(0, 100);
            int complexityChance = Random.Range(0, 100);

            foreach (TaskType type in types.Keys)
                if (types[type].Chance > typeChance)
                {
                    Type = type;
                    break;
                }

            foreach (TaskComplexity complexity in complexities.Keys)
                if (complexities[complexity].Chance > complexityChance)
                {
                    Complexity = complexity;
                    break;
                }

            Award += types[Type].Award + complexities[Complexity].Award;
            launcher = types[Type].Launcher;
        }
    }

    [SerializeField] VisualTreeAsset taskStyle;

    VisualElement UI_currentTask;

	public TaskObject currentTask;
    List<TaskObject> tasks = new List<TaskObject>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
            tasks.Add(new TaskObject());

        VisualElement root = GameObject.Find("UIDocument").GetComponent<UIDocument>().rootVisualElement;

        UI_currentTask = root.Q<VisualElement>("CurrentTaskStyle");
        UI_currentTask.style.display = DisplayStyle.None;

		ListView UI_taskListView = root.Q<ListView>("TaskList");
        UI_taskListView.itemsSource = tasks;
        UI_taskListView.makeItem = () =>
        {
            var element = taskStyle.CloneTree();

            if(currentTask == null)
            {
                element.Q<Button>("AcceptButton").clickable.clicked += delegate
                {
                    int elementIndex = (int)element.userData;

                    TaskObject task = tasks[elementIndex];

                    currentTask = task;
                    tasks.RemoveAt(elementIndex);
                    UI_taskListView.Rebuild();
                };

                element.Q<Button>("DeleteButton").clickable.clicked += delegate
                {
                    int elementIndex = (int)element.userData;

                    tasks.RemoveAt(elementIndex);
                    tasks.Add(new TaskObject());
                    UI_taskListView.Rebuild();
                };
            }
            return element;
        };
        UI_taskListView.bindItem = (element, index) =>
        {
            element.userData = index;

            TaskObject task = tasks[index];

            element.Q<Label>("Type").text = task.Type.ToString();
            element.Q<Label>("Complexity").text = task.Complexity.ToString();
            element.Q<Label>("Award").text = task.Award.ToString();
        };


        ScrollView UI_TaskListView_ScrollView = UI_taskListView.Q<ScrollView>();
        UI_TaskListView_ScrollView.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
        UI_TaskListView_ScrollView.verticalScrollerVisibility = ScrollerVisibility.Hidden;

        Label UI_Money_Label = root.Q<Label>("Money");
        UI_Money_Label.text = "$" + PlayerPrefs.GetInt("Money").ToString();
        OnMoneyChanged += delegate { UI_Money_Label.text = $"${PlayerPrefs.GetInt("Money")}"; };
    }

    float currentTimeScale = 1;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentTimeScale == 0)
            {
                currentTimeScale = 1;
                Time.timeScale += 0.05f;
            }
            else currentTimeScale = 0;
        }
        Time.timeScale = Mathf.MoveTowards(Time.timeScale, currentTimeScale, Time.deltaTime * 5);


        if (currentTask != null)
            currentTask.LaunchTask();

        if(currentTask == null && UI_currentTask.style.display == DisplayStyle.Flex)
			UI_currentTask.style.display = DisplayStyle.None;
		else if (currentTask != null && UI_currentTask.style.display == DisplayStyle.None)
        {
            UI_currentTask.style.display = DisplayStyle.Flex;
			UI_currentTask.Q<Label>("Type").text = currentTask.Type.ToString();
            UI_currentTask.Q<Label>("Complexity").text = currentTask.Complexity.ToString();
            UI_currentTask.Q<Label>("Award").text = currentTask.Award.ToString();
		}
	}

    public event System.Action OnMoneyChanged;
    public void AddMoney(int amount)
    {
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") +  amount);
        OnMoneyChanged?.Invoke();
    }

    IEnumerator DestroyCenter(GameObject center)
    {
        Debug.Log("Start");
        yield return new WaitForSeconds(5f);
        Debug.Log("End");
        Destroy(center.gameObject);
    }

    public class Launchers
    {
        GameObject player = GameObject.Find("Player");
        TaskManager taskManager = GameObject.Find("TaskManager").GetComponent<TaskManager>();

        private enum step
        {
            initial, one, two
        }
        step currentStep = step.initial;

        public class Arrow
        {
			GameObject player = GameObject.Find("Player");

            private GameObject arrow;

			public Arrow()
            {
                GameObject arrowPrefab = Resources.Load<GameObject>("Prefabs/Arrow");
                arrow = Instantiate(arrowPrefab, player.transform);
            }

			public void Rotate(GameObject from, GameObject to)
			{
				Vector3 fromPos = from.transform.position;
				Vector3 toPos = to.transform.position;
				Quaternion currentRotate = arrow.transform.rotation;
				Quaternion finallyRotate = Quaternion.LookRotation(toPos - fromPos);
                float rotateSpeed = 5 * Time.deltaTime;

				arrow.transform.position = fromPos;
				arrow.transform.rotation = Quaternion.Lerp(currentRotate, finallyRotate, rotateSpeed);
			}

            public void Clear()
            {
                Destroy(arrow);
            }
		}

        class DeliveryParameter
        {
            public int distance { get; }
            public int award { get; }

            public DeliveryParameter(int distance, int award)
            {
                this.distance = distance;
                this.award = award;
            }
        }

        Dictionary<TaskComplexity, DeliveryParameter> deliveryComplexity =
            new Dictionary<TaskComplexity, DeliveryParameter>()
            {
                { TaskComplexity.Low, new DeliveryParameter(600, 500) },
                { TaskComplexity.Medium, new DeliveryParameter(800, 600) },
                { TaskComplexity.High, new DeliveryParameter(1000, 800) },
            };

        GameObject unLoadingCenter;
        GameObject loadingCenter;
        /*GameObject arrowPrefab = Resources.Load<GameObject>("Prefabs/Arrow");*/
        Arrow arrow;
        public void LaunchDelivery(TaskObject task)
        {
            if (currentStep == step.initial)
            {
                unLoadingCenter = Instantiate(Resources.Load<GameObject>("Prefabs/Asteroid_Conveyor"));
                unLoadingCenter.transform.position = player.transform.position
                    + Random.insideUnitSphere * deliveryComplexity[task.Complexity].distance;

                loadingCenter = Instantiate(Resources.Load<GameObject>("Prefabs/Asteroid_Conveyor"));
                loadingCenter.transform.position = unLoadingCenter.transform.position
                    + Random.onUnitSphere * deliveryComplexity[task.Complexity].distance;

                arrow = new Arrow();

                currentStep = step.one;
            }

            if (currentStep == step.one)
            {
                float distance = Vector3.Distance(player.transform.position, unLoadingCenter.transform.position);

                arrow.Rotate(player, unLoadingCenter);

                if (distance < 20)
                    currentStep = step.two;
            }

            if (currentStep == step.two)
            {
                float distance = Vector3.Distance(player.transform.position, loadingCenter.transform.position);

				arrow.Rotate(player, loadingCenter);

				if (distance < 20)
                {
                    int award = task.Award;
                    taskManager.AddMoney(award);
                    Destroy(unLoadingCenter);
                    taskManager.StartCoroutine(taskManager.DestroyCenter(loadingCenter));
                    arrow.Clear();
                    taskManager.currentTask = null;
                    taskManager.tasks.Add(new TaskObject());
                    currentStep = step.initial;
                }
            }
            
        }

        public void LaunchExtraction(TaskComplexity complexity)
        {
            Debug.Log(complexity.ToString());
        }

    }
}
