import 'package:flutter/material.dart';

import '../../services/game_api_service.dart';

class CreateRoomScreen extends StatefulWidget {
  const CreateRoomScreen({super.key});

  @override
  State<CreateRoomScreen> createState() => _CreateRoomScreenState();
}

class _CreateRoomScreenState extends State<CreateRoomScreen> {
  final TextEditingController _playerNameController = TextEditingController();

  final GameApiService _gameApiService = GameApiService();

  bool _isLoading = false;
  String? _errorMessage;

  @override
  void dispose() {
    _playerNameController.dispose();
    super.dispose();
  }

  Future<void> _createRoom() async {
    final playerName = _playerNameController.text.trim();

    if (playerName.isEmpty) {
      setState(() {
        _errorMessage = 'Please enter your name.';
      });
      return;
    }

    setState(() {
      _isLoading = true;
      _errorMessage = null;
    });

    try {
      final result = await _gameApiService.createGame(playerName);

      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Room created: ${result.roomCode}')),
      );
    } catch (e) {
      if (!mounted) return;

      setState(() {
        _errorMessage = e.toString();
      });
    } finally {
      if (mounted) {
        setState(() {
          _isLoading = false;
        });
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Create Room')),
      body: Padding(
        padding: const EdgeInsets.all(24),
        child: Column(
          children: [
            TextField(
              controller: _playerNameController,
              decoration: const InputDecoration(
                labelText: 'Your Name',
                border: OutlineInputBorder(),
              ),
            ),

            if (_errorMessage != null) ...[
              const SizedBox(height: 12),
              Text(_errorMessage!, style: const TextStyle(color: Colors.red)),
            ],

            const SizedBox(height: 24),

            SizedBox(
              width: double.infinity,
              height: 50,
              child: ElevatedButton(
                onPressed: _isLoading ? null : _createRoom,
                child: _isLoading
                    ? const CircularProgressIndicator()
                    : const Text('CREATE ROOM'),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
