import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { GoogleGenerativeAI } from '@google/generative-ai';
import { ShopService } from 'src/app/core/services/shop.service';
import { Message } from 'src/app/shared/models/message';
import { AssistantEntries } from './assistant-entry';

export const GEMINI_API_KEY = 'AIzaSyB6Bhe3dIyrplmVr02ZAmT5lgAKj_2s_oI';
export const GEMINI_MODEL_2_0_FLASH_LITE = 'gemini-2.0-flash-lite';

@Component({
  standalone: true,
  selector: 'app-ai-assistent',
  imports: [
    MatFormField,
    MatLabel,
    CommonModule,
    FormsModule,
    MatButton,
    MatInput
  ],
  templateUrl: './ai-assistent.component.html',
  styleUrl: './ai-assistent.component.scss'
})
export class AiAssistentComponent implements OnInit {
  private shopService = inject(ShopService)
  private entries = inject(AssistantEntries)

  userMessage: string = '';
  messages: Message[] = [];

  isLoading: boolean = false;

  validAnswer = false;
  errorMessage = '';
  cat: 'self' | 'item' | 'delivery-method';

  ngOnInit(): void {
    const aiInitMessage: Message = {
      text: 'Hi there! How may i assist you ?',
      sender: 'ai'
    }
    this.messages.push(aiInitMessage);
  }

  inputChanged(e: KeyboardEvent) {
    if (e.key == "Enter") {
      this.ask_question()
    }
    if (this.userMessage != "") {
      this.errorMessage = ""
    }
  }

  async ask_question() {
    if (this.userMessage == "") {
      this.errorMessage = "Please ask the Flash something"
      return
    }
    await this.ask(this.userMessage);
  }

  async ask(question_to_ask) {
    this.messages.push({
      text: question_to_ask,
      sender: 'user'
    })

    const api_key = GEMINI_API_KEY;
    if (api_key.length === 0) {
      this.errorMessage = "Miss API key";
      return
    }

    let prompt = `
    I'm passing you a list of  guidlines at the end of this prompt.\n
    Here is the customer’s question: ${question_to_ask}.
    Here is the topic: ${this.cat ?? ''} .
    Please answer the customer's question based on the entries provided and consider any context from previous chat logs.
    Do not response anything to show that you understand these instructions.

    PREVIOS CHAT LOG:
    ${this.seralizedMessage(this.messages)}

    Here are the entries:\n
    `;

    for (let entry of this.entries.getEntries('self')) {
      prompt += `${entry.entry}\n\n`
    }

    console.log(prompt.toString())

    this.isLoading = true;
    const geminiOutput = await this.callGemini(prompt);

    if (geminiOutput === '-1') {
      this.errorMessage = "Something go wrong. Check clg";
    } else if (geminiOutput === '-2') {
      this.errorMessage = "API key is invalid.";
    } else {
      this.errorMessage = '';
      this.messages.push({
        text: geminiOutput,
        sender: 'ai'
      })
    }
    this.isLoading = false;
    this.userMessage = '';

  }

  async callGemini(prompt: string) {
    const genAI = new GoogleGenerativeAI(GEMINI_API_KEY);

    const model = genAI.getGenerativeModel({
      model: GEMINI_MODEL_2_0_FLASH_LITE
    });
    try {
      const result = await model.generateContent(prompt);
      const response = await result.response;
      const text = response.text();
      console.log(text)
      return text;
    } catch (e: any) {
      if (e.message.toLowerCase().includes("api key")) {
        return '-2'
      } else {
        return '-1'
      }
    }
  }

  seralizedMessage(messages: any) {
    return JSON.stringify(messages);
  }
}
