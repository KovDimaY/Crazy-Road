using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrazyRoad
{
    class GameConstants
    {
        //players support
        public enum Player { One, Two };
        public const int PLAYER_MOVEMENT_SPEED = 2;

        // resolution support
        public const int WINDOW_WIDTH = 1000;
        public const int WINDOW_HEIGHT = 650;

        //score support
        public const int SCORE_GET_FINISH = 100;
        public const int SCORE_COMPETITOR_DIES = 10;

        //lines
        public const int FAST_LINE = 2;
        public const int MEDIAN_LINE = 1;
        public const int SLOW_LINE = 0;
        public const int LINE1_POSITION = 510;
        public const int LINE2_POSITION = 430;
        public const int LINE3_POSITION = 360;
        public const int LINE4_POSITION = 280;
        public const int LINE5_POSITION = 200;
        public const int LINE6_POSITION = 130;

        //cars
        public enum Direction { Left, Right };
        public const int CARS_NUMBER = 10;
        public const int FAST_CAR_SPEED = 7;
        public const int MEDIAN_CAR_SPEED = 5;
        public const int SLOW_CAR_SPEED = 3;

        // initial positions support
        public const int PLAYER_1_START_POSITION_X = (int)(WINDOW_WIDTH * 0.75);
        public const int PLAYER_2_START_POSITION_X = (int)(WINDOW_WIDTH * 0.25);
        public const int PLAYERS_START_POSITION_Y = WINDOW_HEIGHT - 50;
        public const int ROAD_OFFSET = 70;

        //score position support
        public const int PLAYER1_SCORE_POSITION_X = 30;
        public const int PLAYER1_SCORE_POSITION_Y = 10;
        public const int PLAYER2_SCORE_POSITION_X = GameConstants.WINDOW_WIDTH / 2 + GameConstants.PLAYER1_SCORE_POSITION_X;
        public const int PLAYER2_SCORE_POSITION_Y = GameConstants.PLAYER1_SCORE_POSITION_Y;

        //introduction support
        public const int INTRO_FIRST_DIGIT_DELAY = 1000;
        public const int INTRO_SECOND_DIGIT_DELAY = 2000;
        public const int INTRO_THIRD_DIGIT_DELAY = 3000;

        //time delays
        public const int PLAYERS_START_DELAY = 3000;
        public const int CARS_SPAWN_DELAY = 500;
    }
}
