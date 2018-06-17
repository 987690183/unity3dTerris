local _Class = {}
 
function Class(super)
	local Class_type = {}
	Class_type.__init = false
	Class_type.__delete = false
	Class_type.super = super
	Class_type.superclass = _Class[super]

	Class_type.New = function(...)
		local obj = {}
		obj.class_type = Class_type
		setmetatable(obj, { __index = _Class[Class_type]})
		-- 注册一个create方法
		do
			local create
			create = function(c, ...)
				if c.super then
					create(c.super, ...)
				end
				if c.__init then
					c.__init(obj, ...)
				end
			end
			create(Class_type, ...)
		end

		-- 注册一个delete方法
		obj.Destory = function(self)
			local now_super = self.class_type
			while now_super ~= nil do
				if now_super.__delete then
					now_super.__delete(self)
				end
				now_super = now_super.super
			end
		end

		return obj
	end

	local vtbl = {}

	_Class[Class_type] = vtbl

	setmetatable(Class_type, {__newindex=
		function(t, k, v)
			vtbl[k] = v
		end
	})
 
 	-- 写时复制
	if super then
		setmetatable(vtbl, {__index=
			function(t, k)
				local ret = _Class[super][k]
				vtbl[k] = ret
				return ret
			end
		})
	end
 
	return Class_type
end