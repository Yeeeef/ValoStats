﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using ValoStats.Models;
using ValoStats.ViewModels.Helpers;

namespace ValoStats.ViewModels.DTOs
{
    public class MatchDTO
    {
        public static Config Config = FileHelper.ReadConfig();
        public static string name = Config.Name;
        public static Match OneMatchResponseToDTO(MatchResponse Data)
        {
            Metadata Match = Data.data.First().metadata;
            return new Match()
            {
                map = Match.map,
                game_length = Match.game_length,
                rounds_played = Match.rounds_played,
                mode = Match.mode,
            };
        }

        public static Match DatumToMatchDTO(Datum Data)
        {
            return new Match()
            {
                map = Data.metadata.map,
                game_length = Data.metadata.game_length,
                rounds_played = Data.metadata.rounds_played,
                mode = Data.metadata.mode,
            };
        }


        public static List<Datum> MatchesResponseToDatums(MatchResponse MatchResponse)
        {
            List<Datum> _ = new();
            foreach (Datum D in MatchResponse.data)
            {
                _.Add(D);
            };
            return _.ToList();
        }

        public static Datum MatchResponseToDatum(MatchResponse MatchResponse)
        {
            return new Datum()
            {
                metadata = MatchResponse.data.First().metadata,
                players = MatchResponse.data.First().players,
                observers = MatchResponse.data.First().observers,
                coaches = MatchResponse.data.First().coaches,
                teams = MatchResponse.data.First().teams,
                rounds = MatchResponse.data.First().rounds,
                kills = MatchResponse.data.First().kills,
            };
        }

        public static PlayedMatch DatumToPlayedMatch(Datum Datum)
        {
            string result = "";
            AllPlayer player = Datum.players.all_players.Find(p => p.name == name);
            Team team = Datum.teams.blue.name == name ? Datum.teams.blue : Datum.teams.red;
            return new PlayedMatch()
            {
                Map = Datum.metadata.map,
                Player = player,
                Score = team.rounds_won != null ? $"{team.rounds_won}-{team.rounds_lost}" : "",
                Kills = player.stats.kills,
                Deaths = player.stats.deaths,
                KD = $"{player.stats.kills}/{player.stats.deaths}",
                Mode = Datum.metadata.mode,
                Region = Datum.metadata.region,
                Platform = Datum.metadata.platform,
                Team = team,
                Agent = player.character,
                Result = team.has_won
            };
        }
    }
}