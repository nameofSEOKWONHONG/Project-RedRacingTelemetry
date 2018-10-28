namespace Project_RedRacingTelemetry
{
    public class Test {
        public void RunUdp() {
            UDPSocket s = new UDPSocket();
            s.Server("127.0.0.1", 20777);

            UDPSocket c = new UDPSocket();
            c.Client("127.0.0.1", 20777);
            c.Send("TEST!");
        }
    }
}