using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class wALLBbEAKER : PhysicsGame
{
    const double nopeus = 200;
    const double hyppyNopeus = 200;
    const int RUUDUN_KOKO = 40;

    PlatformCharacter pelaaja1;

    Image pelaajanKuva = LoadImage("BREAKER.player1");
    Image tahtiKuva = LoadImage("tahti");
    Image WALLkuva = LoadImage("WALL.no-effect");
    Image WALLBkuva = LoadImage("WALL.broke");
    Image Murderikuva = LoadImage("MURDER.bot+player2");

    PhysicsObject WALL;

    SoundEffect maaliAani = LoadSoundEffect("Blip_Select");
    SoundEffect NUKEaani = LoadSoundEffect("NUKE");

    public override void Begin()
    {
        Gravity = new Vector(0, -1000);

        LuoKentta();
        LisaaNappaimet();

        Camera.Follow(pelaaja1);
        Camera.ZoomFactor = 1.2;
        Camera.StayInLevel = true;
    }

    void LuoKentta()
    {
        TileMap kentta = TileMap.FromLevelAsset("kentta1");
        kentta.SetTileMethod('#', LisaaTaso);
        kentta.SetTileMethod('L', LisaaTahti);
        kentta.SetTileMethod('N', LisaaPelaaja);
        kentta.SetTileMethod('W', LisaaWALL);
        kentta.SetTileMethod('H', LisaaMurderi);
        kentta.Execute(RUUDUN_KOKO, RUUDUN_KOKO);
        Level.CreateBorders();
        Level.Background.CreateGradient(Color.White, Color.SkyBlue);
    }

    void LisaaTaso(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject taso = PhysicsObject.CreateStaticObject(leveys, korkeus);
        taso.Position = paikka;
        taso.Color = Color.Green;
        Add(taso);
    }

    void LisaaWALL(Vector paikka, double leveys, double korkeus)
    {

        WALL = PhysicsObject.CreateStaticObject(leveys, korkeus);
        WALL.Position = paikka;
        WALL.Image = WALLkuva;
        Add(WALL);
    }

    void LisaaMurderi(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject Murderi = PhysicsObject.CreateStaticObject(leveys, korkeus);
        Murderi.Position = paikka;
        Murderi.Image = Murderikuva;
        Add(Murderi);
        

    }

    void LisaaTahti(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject tahti = PhysicsObject.CreateStaticObject(leveys, korkeus);
        tahti.IgnoresCollisionResponse = true;
        tahti.Position = paikka;
        tahti.Image = tahtiKuva;
        tahti.Tag = "tahti";
        Add(tahti);
    }

    void LisaaPelaaja(Vector paikka, double leveys, double korkeus)
    {
        pelaaja1 = new PlatformCharacter(leveys, korkeus);
        pelaaja1.Position = paikka;
        pelaaja1.Mass = 4.0;
        pelaaja1.Image = pelaajanKuva;
        AddCollisionHandler(pelaaja1, "tahti", TormaaTahteen);
        Add(pelaaja1);
    }

    void LisaaNappaimet()
    {
        Keyboard.Listen(Key.F1, ButtonState.Pressed, ShowControlHelp, "Näytä ohjeet");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");

        Keyboard.Listen(Key.Left, ButtonState.Down, Liikuta, "Liikkuu vasemmalle", pelaaja1, -nopeus);
        Keyboard.Listen(Key.Right, ButtonState.Down, Liikuta, "Liikkuu vasemmalle", pelaaja1, nopeus);
        Keyboard.Listen(Key.Up, ButtonState.Pressed, Hyppaa, "Pelaaja hyppää", pelaaja1, hyppyNopeus);
        Keyboard.Listen(Key.N, ButtonState.Pressed, NUKE, "POKS");
        //Keyboard.listen(Key.Space ButtonState.Pressed WallBreake, "CRUNKS");

        ControllerOne.Listen(Button.Back, ButtonState.Pressed, Exit, "Poistu pelistä");

        ControllerOne.Listen(Button.DPadLeft, ButtonState.Down, Liikuta, "Pelaaja liikkuu vasemmalle", pelaaja1, -nopeus);
        ControllerOne.Listen(Button.DPadRight, ButtonState.Down, Liikuta, "Pelaaja liikkuu oikealle", pelaaja1, nopeus);
        ControllerOne.Listen(Button.A, ButtonState.Pressed, Hyppaa, "Pelaaja hyppää", pelaaja1, hyppyNopeus);

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
    }

    void Liikuta(PlatformCharacter hahmo, double nopeus)
    {
        hahmo.Walk(nopeus);
    }

    void Hyppaa(PlatformCharacter hahmo, double nopeus)
    {
        hahmo.Jump(nopeus);
    }

    void TormaaTahteen(PhysicsObject hahmo, PhysicsObject tahti)
    {
        maaliAani.Play();
        MessageDisplay.Add(":)");
        tahti.Destroy();
    }

    void NUKE()
    {
        WALL.Destroy();
        NUKEaani.Play();
        MessageDisplay.Add("POKS!POKS!POKS!POKS");
}
}