import 'package:flutter/material.dart';

import 'screens/lobby/lobby_screen.dart';

void main() {
  runApp(const UnoNoMercyApp());
}

class UnoNoMercyApp extends StatelessWidget {
  const UnoNoMercyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      title: 'UNO No Mercy',
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(seedColor: Colors.red),
        useMaterial3: true,
      ),
      home: const LobbyScreen(),
    );
  }
}
