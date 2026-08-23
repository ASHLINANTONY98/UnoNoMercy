import 'package:flutter/material.dart';

import '../room/create_room_screen.dart';

class LobbyScreen extends StatelessWidget {
  const LobbyScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('UNO No Mercy')),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            const Text(
              'UNO NO MERCY',
              style: TextStyle(fontSize: 32, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 40),
            SizedBox(
              width: 220,
              height: 50,
              child: ElevatedButton(
                onPressed: () {
                  Navigator.push(
                    context,
                    MaterialPageRoute(
                      builder: (context) => const CreateRoomScreen(),
                    ),
                  );
                },
                child: const Text('CREATE ROOM'),
              ),
            ),
            const SizedBox(height: 16),
            SizedBox(
              width: 220,
              height: 50,
              child: OutlinedButton(
                onPressed: () {},
                child: const Text('JOIN ROOM'),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
