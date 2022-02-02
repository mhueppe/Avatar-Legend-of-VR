public interface IState
{
    public void OnStateEnter();
    public void Tick();
    public void OnStateExit();
}
