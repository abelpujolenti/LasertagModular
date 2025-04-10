//Keys
enum class PacketKeys
{
  TEST = 1
};


class Test
{
  public:
    String sos;
    int puto;
    Test(String _sos, int _puto) { sos = _sos; puto = _puto; }
    void PrintThings() {Serial.println("THINGS INSIDE PACKET ARE: " + sos + ", " + puto);}
};