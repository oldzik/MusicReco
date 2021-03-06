﻿using MusicReco.App.Abstract;
using MusicReco.App.Concrete;
using MusicReco.App.HelpersForManagers;
using MusicReco.App.Managers;
using MusicReco.Domain.Entity;
using System;
using System.Dynamic;
using System.Globalization;
using System.Net.WebSockets;
using System.Xml.Serialization;

namespace MusicReco
{
    class Program
    {
        static void Main(string[] args)
        {
            MenuView menuView = new MenuView();
            ISongService songService = new SongService();
            IPlaylistService playlistService = new PlaylistService();
            SongManager songManager = new SongManager(menuView, songService);
            PlaylistManager playlistManager = new PlaylistManager(menuView, songService, playlistService);
            bool running = true;

            while (running)
            {
                Console.Clear();
                menuView.ShowMainMenu();
                var operation = Console.ReadKey();
                Console.WriteLine();

                bool again = true;
                switch (operation.KeyChar)
                {
                    case '1':
                        Console.Clear();
                        var newSong = songManager.CreateNewSong();
                        int newSongId = songManager.AddNewSong(newSong);
                        if (newSongId != -1)
                            Console.ReadKey();
                        break;
                    case '2':                        
                        do
                        {
                            Console.Clear();
                            int choice = songManager.Recommend();
                            again = songManager.RecommendationSwitcher(choice);
                        } while (again);                       
                        break;
                    case '3':                        
                        do
                        {
                            Console.Clear();
                            string title = songManager.ChooseSongToBeLiked();
                            int songId = songManager.LikeChosenSong(title);
                            again = songManager.SuccessfulOrFailedLikeInfo(songId);
                        }while(again);
                        break;
                    case '4':
                        do
                        {
                            Console.Clear();
                            string title = songManager.ChooseSongToShowDetails();
                            var song = songManager.SearchSongToShowDetails(title);
                            again = songManager.ShowDetails(song);
                        } while (again);
                        break;
                    case '5':
                        do
                        {
                            Console.Clear();
                            int choice = playlistManager.ChooseToCreateOrAddPlaylist();
                            again = playlistManager.CreateNewOrAddSwitcher(choice);
                        } while (again);
                        break;
                    case '6':
                        int playlistId;
                        do
                        {
                            Console.Clear();
                            playlistId = playlistManager.ChoosePlaylistToShow();
                            playlistManager.ShowPlaylist(playlistId);

                        } while (playlistId != -1);
                        break;
                    case '7':
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Such action doesn't exist. Press any key to try again...");
                        Console.ReadKey();
                        break;
                }
            }
        }              
    }
}
