# 📦 Mecânica

## 🔁 Loop Principal

O loop principal do jogo segue a estrutura clássica de jogos 2D, onde cada ciclo realiza as seguintes etapas:

- Captura de entrada do jogador (movimentação, ataques, uso de poções).
- Atualização dos estados dos personagens, inimigos e interações com o ambiente.
- Verificação de colisões e aplicação de danos.
- Renderização dos elementos visuais na tela.
- Controle da taxa de quadros para manter a fluidez da jogabilidade.

## 🧍‍♀️ Atores e Seus Componentes

### Principais Atores

- **Protagonista:** Possui atributos como vida, poções e habilidades especiais.
- **Inimigos (Zumbis e Chefes):** Tipos variados, com padrões únicos de movimento e ataque.
- **NPCs (Golem):** Guiam a narrativa e oferecem dicas ao jogador.
- **Objetos Interativos:** Usados na progressão.

### Componentes dos Atores

- **Movimentação:** Baseada em física (com colisões).
- **Estado:** Parado, andando ou atacando.
- **IA dos Inimigos:** Comportamentos programados para adicionar desafio.

## 🎨 Sprites

Todos os elementos visuais (personagens, objetos, efeitos) são representados por sprites 2D, utilizando **spritesheets** com animações por quadro.  
Os sprites de ambiente são do jogo *Stardew Valley*, enquanto os personagens são customizados através do https://liberatedpixelcup.github.io/Universal-LPC-Spritesheet-Character-Generator. Os coletáveis, poções e UI ainda não foram decididos.

## 🌲 Background

Os cenários usam **camadas em parallax**, criando profundidade visual.  
Ambientes como floresta e laboratório têm estilos visuais distintos e reforçam a imersão.

---

## 🎮 Elementos Formais do Jogo

### 🧭 Padrão de Interação

- **Teclado:** Movimentação e dash.
- **Mouse:** Mira e lançamento de poções.
- **Interface:** Interações com objetos/NPCs.

### 🎯 Objetivo do Jogo

- **Objetivo:** Coletar três ingredientes, criar o Elixir da Vida e derrotar/salvar o mestre.
- **Vitória:** Derrotar o mestre zumbi e aplicar o elixir.
- **Derrota:** Vida zerada sem recursos de cura. Temporária: volta para o último checkpoint.

### 📏 Regras do Jogo

- Apenas uma poção ativa por vez.
- Poções têm cooldown.
- Máximo de 6 vidas (pode aumentar com equipamentos).
- Inimigos têm fraquezas/resistências.
- Salvamento só ocorre ao escrever no diário.

### 🔁 Procedimentos do Jogo

- Mover-se e explorar os mapas.
- Usar dash e lançar poções para derrotar inimigos.
- Resolver puzzles e coletar ingredientes.
- Falar com NPCs para progredir.
- Criar o Elixir da Vida no caldeirão.

### 💎 Recursos do Jogo

- Poções: Gelo, Fogo, Raio, Explosão.
- Vida: Representada por corações.
- Itens de melhoria: Peitoral, Bota, Crossbow.
- Ingredientes mágicos.
- Checkpoints: Diários.

### 🚧 Limites do Jogo

- Fases acessíveis apenas após completar as anteriores.
- Alcance e ações limitadas por cooldowns.
- Áreas bloqueadas exigem habilidades específicas para acesso.

### 🏁 Resultados do Jogo

#### Vitória salvando o mestre:
- Cutscene mostra o mestre sendo curado.
- Epílogo revela o mundo em paz.

#### Vitória matando o mestre:
- Cutscene mostra o mestre dizendo que só queria dominar o mundo.
- Epílogo revela o mundo em paz.

#### Derrota:
- A protagonista desmaia e retorna ao último checkpoint.

