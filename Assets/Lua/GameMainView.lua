
GameMainView = Class()

function GameMainView:__init()
	self.timer = nil

	self.objAgainGame = UnityEngine.GameObject.Find("Canvas/AgainGame")
	self.objBeginGame = UnityEngine.GameObject.Find("BeginGame")
	self.objSprite = UnityEngine.GameObject.Find("sprite")

	self.button = UnityEngine.GameObject.Find("Canvas/BeginGame/Button"):GetComponent("Button")
	self.button.onClick:AddListener(function()
		self:ButtonHandler()
	end)

	self.againButton = UnityEngine.GameObject.Find("Canvas/AgainGame/btnPanle/again"):GetComponent("Button")
	self.againButton.onClick:AddListener(self.againButtonHandler)
	self.objAgain = UnityEngine.GameObject.Find("Canvas/AgainGame/btnPanle/again")

	self.quitButton = UnityEngine.GameObject.Find("Canvas/AgainGame/btnPanle/quit"):GetComponent("Button")
	self.quitButton.onClick:AddListener(self.quitButtonHandler)

	self.objAgainGame:SetActive(false)

	self:StartTimer()
end

function GameMainView:StartTimer()
	FunctionTeris.getInstance():InitGame(self.objAgainGame.transform, self.objSprite);

	if not self.timer then
		self.timer = function ()self:OnUpdateAction() end
		UpdateBeat:Add(self.timer)
	end
end

-- 每一帧更新
function GameMainView:OnUpdateAction()
	FunctionTeris.getInstance():DealGameFunction()

    if FunctionTeris.getInstance().isGameStop then
    	if not self.objAgain.activeSelf then
    		self.objAgain:SetActive(true)
    	end
    else
    end
end

function GameMainView:StopTimer()
	if self.timer then
		UpdateBeat:Remove(self.timer);
		self.timer = nil;
	end
end


function GameMainView:ButtonHandler()
	self.objAgainGame:SetActive(true)
    UnityEngine.GameObject.Destroy(self.objBeginGame)
    self.objAgain:SetActive(false)
    FunctionTeris.getInstance():BeginGame()
end

function GameMainView:againButtonHandler()
end

function GameMainView:quitButtonHandler()
end


function GameMainView:__delete()
end