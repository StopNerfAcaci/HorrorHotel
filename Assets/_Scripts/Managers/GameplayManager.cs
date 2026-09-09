using System;
using GlobalSettings;
using HSM;
using TMPro;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;
    public event Action<string> OnDayPhaseChanged;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private CameraController camController;
    [SerializeField] private GlobalSettings.Gameplay gameplay;
    [SerializeField] private PlayerStateDriver player;
    public PlayerStateDriver Player => player;
    public UIManager UIManager => uiManager;
    private DayPhase currentDayPhase;
    private int currentDayIndex = -1;

    public GameStateMachine FSM { get; set; }
    private void Awake()
    {
        Instance = this;
        FSM = new GameStateMachine(this);
        if (gameplay.TryLoadDay(out currentDayPhase))
        {
            currentDayIndex = FindIndex(currentDayPhase);
        }
        // Inventory.Get().LoadInventory();
        camController.SwitchCam(false);
    }

    private void Start()
    {
        if (currentDayIndex < 0)
        {
            currentDayIndex = 0;
            currentDayPhase = gameplay.Days[0];
            gameplay.SaveDay(currentDayPhase);
        }
        OnDayPhaseChanged?.Invoke(currentDayPhase.GetDayString());
    }

    private void OnEnable()
    {
        if (uiManager)
        {
            uiManager.OnStartGame += StartGame;
        }
    }

    private void StartGame()
    {
        SwitchToPlayerCam();
        FSM.ChangeState(GameStateType.InGame);
    }

    private void Update()
    {
        FSM.Tick();
    }
 
    private void FixedUpdate()
    {
        FSM.FixedTick();
    }

    private void LateUpdate()
    {
        FSM.LateTick();
    }

    private void OnDestroy()
    {
        FSM.Dispose();
    }

    internal void LateUpdateInternal()
    {
        camController.OnLateUpdate(Time.deltaTime);
    }

    private int FindIndex(DayPhase phase)
    {
        var days = gameplay.Days;
        for (int i = 0; i < days.Length; i++)
        {
            if (days[i].day == phase.day && days[i].isDaytime == phase.isDaytime)
                return i;
        }

        return -1;
    }

    public bool CanMoveNextPhase()
    {
        foreach (var item in currentDayPhase.requireItems)
        {
            if (!Inventory.Get().CheckHasKey(item)) return false;
        }

        return true;
    }

    public void HandleNextPhase()
    {
        currentDayIndex++;
        if (currentDayIndex >= gameplay.Days.Length)
        {
            Debug.Log("Reached end of days.");
            return;
        }

        currentDayPhase = gameplay.Days[currentDayIndex];
        gameplay.SaveDay(currentDayPhase);
        OnDayPhaseChanged?.Invoke(currentDayPhase.GetDayString());
    }
    [ContextMenu("Reset data")]
    public void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    private void SwitchToPlayerCam()
    {
        camController.SwitchCam(true);
    }
    
}