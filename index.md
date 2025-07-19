---
_layout: landing
---

# 🏗️ Tycoon Project

> **Sistema Modular de Grid para Unity**  
> Arquitetura robusta e escalável para jogos do gênero tycoon

[![Unity](https://img.shields.io/badge/Unity-6-black?style=flat-square&logo=unity)](https://unity.com/)
[![C#](https://img.shields.io/badge/C%23-9.0-blue?style=flat-square&logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/License-MIT-green?style=flat-square)](LICENSE)

---

## 🎯 **Visão Geral do Projeto**

O **Tycoon Project** é um sistema completo de grid desenvolvido em Unity para criação de jogos do gênero tycoon. Implementa uma arquitetura modular robusta com funcionalidades avançadas de posicionamento de tiles, efeitos visuais responsivos e gerenciamento inteligente de recursos.

### ✨ **Características Principais**

- 🏗️ **Arquitetura Modular** - Separação clara de responsabilidades
- 🎮 **Sistema de Grid Inteligente** - Conversão automática de coordenadas
- 🖱️ **Efeitos de Hover Responsivos** - Feedback visual em tempo real
- 🔄 **Object Pool Otimizado** - Performance superior com reutilização de objetos
- 🛒 **Sistema de Loja Intuitivo** - Interface animada e responsiva
- 📱 **Input System Moderno** - Baseado no Unity Input System

---

## 🚀 **Começar Agora**
<div style="display: grid; grid-template-columns: repeat(auto-fit, minmax(250px, 1fr)); gap: 20px; margin: 20px 0;">

<div style="border: 1px solid #e1e5e9; border-radius: 8px; padding: 20px; background: #f8f9fa;">
<h3 style="color: #000000">📚 Guia de Introdução</h3>
<p style="color: #000000">Aprenda os conceitos básicos e configure seu primeiro projeto.</p>
<a href="docs/introduction.md" style="color: #0366d6; text-decoration: none; font-weight: bold;">→ Começar Tutorial</a>
</div>

<div style="border: 1px solid #e1e5e9; border-radius: 8px; padding: 20px; background: #f8f9fa;">
<h3 style="color: #000000">🔧 Referência da API</h3>
<p style="color: #000000">Documentação completa de todas as classes e métodos.</p>
<a href="api/index.md" style="color: #0366d6; text-decoration: none; font-weight: bold;">→ Explorar API</a>
</div>

<div style="border: 1px solid #e1e5e9; border-radius: 8px; padding: 20px; background: #f8f9fa; font-">
<h3 style="color: #000000">🎮 Como Começar</h3>
<p style="color: #000000">Guia rápido para configuração e primeiros passos.</p>
<a href="docs/getting-started.md" style="color: #0366d6; text-decoration: none; font-weight: bold;">→ Setup Rápido</a>
</div>

</div>

---

## 🏗️ **Arquitetura do Sistema**

### 📦 **Componentes Principais**

| Sistema | Funcionalidade | Status |
|---------|----------------|--------|
| **[Grid System](api/index.md#grid-system)** | Gerenciamento central e coordenação | ✅ Completo |
| **[Grid Modules](api/index.md#grid-modules)** | Módulos especializados (Hover, Path, etc.) | ✅ Completo |
| **[Tile System](api/index.md#tile-system)** | Comportamento e pooling de tiles | ✅ Completo |
| **[Shop System](api/index.md#shop-system)** | Interface de loja e seleção | ✅ Completo |

### 🎨 **Funcionalidades Destacadas**

#### **Sistema de Hover Inteligente**
```csharp
// Configuração simples e poderosa
GridManager.Instance.SetHoverEnabled(true);
GridManager.Instance.SetHoverColor(Color.yellow);
```

#### **Object Pool Otimizado**
```csharp
// Reutilização eficiente de objetos
GameObject tile = TilePool.Instance.GetPooledObject();
tile.SetActive(true);
```

#### **Posicionamento Intuitivo**
```csharp
// Sistema de ghost mode para preview
tile.HandleSelection(true); // Ativa modo fantasma
// Clique para posicionar automaticamente
```

---

## 🎮 **Demonstração Visual**

### **Funcionalidades em Ação**

- **🖱️ Hover Effects** - Destaque visual de células disponíveis
- **👻 Ghost Mode** - Preview transparente durante posicionamento  
- **🎨 Animações Suaves** - Transições coordenadas da loja
- **📐 Grid Responsivo** - Conversão automática de coordenadas
- **🔄 Performance Otimizada** - Object pooling para tiles

### **Controles Principais**

| Ação | Controle | Resultado |
|------|----------|-----------|
| **Abrir Loja** | `Tab` | Interface animada de seleção |
| **Selecionar Tile** | `Clique na Loja` | Ativa modo de posicionamento |
| **Posicionar** | `Clique no Grid` | Confirma posição do tile |
| **Hover Visual** | `Mouse sobre Grid` | Destaque de células livres |

---

## 📊 **Tecnologias Utilizadas**

<div style="display: flex; flex-wrap: wrap; gap: 10px; margin: 20px 0;">
<span style="background: #239120; color: white; padding: 5px 10px; border-radius: 4px; font-size: 12px;">Unity 6</span>
<span style="background: #239120; color: white; padding: 5px 10px; border-radius: 4px; font-size: 12px;">C# 9.0</span>
<span style="background: #0366d6; color: white; padding: 5px 10px; border-radius: 4px; font-size: 12px;">Unity Input System</span>
<span style="background: #6f42c1; color: white; padding: 5px 10px; border-radius: 4px; font-size: 12px;">ScriptableObjects</span>
<span style="background: #e36209; color: white; padding: 5px 10px; border-radius: 4px; font-size: 12px;">Object Pooling</span>
<span style="background: #28a745; color: white; padding: 5px 10px; border-radius: 4px; font-size: 12px;">Modular Architecture</span>
</div>

---

## 🏆 **Padrões de Design Implementados**

- **🎯 Singleton Pattern** - GridManager e TilePool
- **🔧 Module Pattern** - Separação de responsabilidades  
- **🔄 Object Pool Pattern** - Otimização de performance
- **👀 Observer Pattern** - Sistema de eventos de input
- **🔄 State Pattern** - Estados bem definidos de tiles

---

## 📈 **Próximos Passos**

<div style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 20px; border-radius: 8px; margin: 20px 0;">
<h3 style="margin-top: 0;">🚀 Explore o Projeto</h3>
<ol style="margin: 0;">
<li><strong>Leia a Introdução</strong> - Entenda os conceitos fundamentais</li>
<li><strong>Configure o Ambiente</strong> - Setup inicial rápido</li>
<li><strong>Explore a API</strong> - Documentação completa dos componentes</li>
<li><strong>Experimente o Sistema</strong> - Teste as funcionalidades interativas</li>
</ol>
</div>

<div style="text-align: center; margin: 40px 0; padding: 20px; background: #f8f9fa; border-radius: 8px;">
<p style="margin: 0; color: #6a737d; font-size: 14px;">
🌟 <strong>Desenvolvido com Unity 6</strong> 🌟<br>
Documentação gerada com DocFX
</p>
</div>
