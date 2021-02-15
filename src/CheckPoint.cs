namespace MachineParts.Scripts
{
    public class CheckPoint : Machine
    {
        public override void ConnectionUpdate()
        {
            if (IsPowered)
            {
                GameManager.instance.Player.transform.position = transform.position;

                if (GameManager.instance.playerStatus.HasStatus(Status.StatusMode.Bubble))
                {
                    GameManager.instance.playerStatus.StateChange();
                }
            }
        }
    }
}
