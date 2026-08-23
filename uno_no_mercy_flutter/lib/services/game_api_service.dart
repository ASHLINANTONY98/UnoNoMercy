import 'dart:convert';

import 'package:http/http.dart' as http;

class GameApiService {
  static const String baseUrl = 'http://192.168.1.73:5285';

  Future<CreateGameResponse> createGame(String playerName) async {
    final response = await http.post(
      Uri.parse('$baseUrl/api/Game/create'),
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
      },
      body: jsonEncode({'playerName': playerName}),
    );

    if (response.statusCode != 200) {
      throw Exception('Failed to create game: ${response.body}');
    }

    final data = jsonDecode(response.body) as Map<String, dynamic>;

    return CreateGameResponse.fromJson(data);
  }
}

class CreateGameResponse {
  final String gameId;
  final String roomCode;

  CreateGameResponse({required this.gameId, required this.roomCode});

  factory CreateGameResponse.fromJson(Map<String, dynamic> json) {
    return CreateGameResponse(
      gameId: json['gameId'] as String,
      roomCode: json['roomCode'] as String,
    );
  }
}
