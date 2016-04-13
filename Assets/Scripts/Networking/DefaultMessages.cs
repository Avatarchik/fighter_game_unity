using UnityEngine;
using System.Collections;

public static class DefaultMessages {

    public const int PING_REQUEST = 44;
    public const int PING_RESPONSE = 45;

    //Login
    public const int LOGIN = 1;
    public const int LOGIN_UNAME_PASS = 2;
    public const int LOGIN_FAILED = 3;
    public const int BAD_REQUEST = 4;

    //Opponent search
    public const int SEARCH = 6;
    public const int SEARCH_CANCEL = 7;
    public const int GAME_FOUND = 8;
    public const int SEARCH_COMMAND_ACCEPTED = 9;
    public const int SEARCH_CANCEL__ACCEPTED = 30;
    public const int SEARCH_GAME = 60;
    public const int SEARCH_1v1 = 2;

    //Shop messages
    public const int SHOP_MESSAGE = 10;
    public const int SHOP_BUY_TEAM = 11;
    public const int SHOP_CANT_BUY_THIS = 12;
    public const int SHOP_BUY_SUCCESSFUL = 13;
    public const int SHOP_GET_ITEMS = 14;
    public const int SHOP_TEAM = 15;

    //Game messages
    public const int NEED_SELECTED_TEAMS = 34;
    public const int GAME_MESSAGE = 19;
    public const int READY_FOR_GAME = 20;
    public const int GAME_POSITION = 62;
    public const int WAITING_FOR_OTHER_PLAYERS = 24;
    public const int GAME_READY = 25;
    public const int ANOTHER_PLAYERS_TURN = 26;
    public const int MOTION_ACCEPTED = 35;
    public const int MOTION_NOT_ACCEPTED = 36;
    public const int GAME_SPAWN_ENEMY = 66;
    public const int GAME_SPAWN_CHARACTER = 67;

    //Request user stats
    public const int USER_STAT_REQUEST = 27;
    public const int USER_STAT_USER = 46;
    public const int USER_STAT_SERVER_VAR = 47;
}
